# AR Furniture Placement

A web-based AR app to visualize and place furniture in your space using your device camera.

**Demo:** [Deployed Link](https://ics-544-ar-furniture.vercel.app)
**Repo:** [github.com/ebtesam-l/ICS544-AR-furniture](https://github.com/ebtesam-l/ICS544-AR-furniture)

---

## 📦 Package Info

| Property | Details |
|----------|---------|
| **Version** | 1.0.0 |
| **Main Package** | `index.html` |
| **Dependencies** | Three.js r128 (CDN), Google Fonts |
| **Language** | HTML5 + Vanilla JavaScript |

---

## 🎯 Target Platform

- **Browsers:** Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- **Mobile:** iOS Safari 14.5+, Chrome Mobile 90+, Android browsers
- **Requirements:** Camera access, 2GB+ RAM
- **No build tool needed** – just open `index.html`

---

## 🚀 Run/Build Steps

```bash
# Option 1: Direct (No Server)
Open index.html in your browser

# Option 2: Local Server
python3 -m http.server 8000
# or
npx http-server

# Option 3: Production (Already Deployed)
Visit https://ics-544-ar-furniture.vercel.app
```

---

## 🎮 Controls

| Mode | Control | Action |
|------|---------|--------|
| **Start** | "Start AR Experience" | Launch camera |
| **Scan** | Tap screen | Detect floor |
| **Place** | Tap floor | Place furniture |
| **Adjust** | Move/Rotate/Scale buttons | Transform furniture |
| | Furniture chips | Select item |
| | Place/Reset buttons | New item / Reset position |

---

## 🛠️ Known Issues

1. **CDN Required** – Needs internet for Three.js
2. **Low-end Devices** – May drop frames on < 2GB RAM


---

## 🎨 Credits

| Asset | License | Source |
|-------|---------|--------|
| **Three.js** | MIT | [github.com/mrdoob/three.js](https://github.com/mrdoob/three.js) |


