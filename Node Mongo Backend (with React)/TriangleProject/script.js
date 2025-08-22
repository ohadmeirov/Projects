function drawTriangle() {
  const x1 = parseFloat(document.getElementById("x1").value);
  const y1 = parseFloat(document.getElementById("y1").value);
  const x2 = parseFloat(document.getElementById("x2").value);
  const y2 = parseFloat(document.getElementById("y2").value);
  const x3 = parseFloat(document.getElementById("x3").value);
  const y3 = parseFloat(document.getElementById("y3").value);

  if ([x1, y1, x2, y2, x3, y3].some(isNaN)) {
    alert("Please fill all coordinates!");
    return;
  }

  const canvas = document.getElementById("canvas");
  const ctx = canvas.getContext("2d");
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  const margin = 80;
  const minX = Math.min(x1, x2, x3);
  const maxX = Math.max(x1, x2, x3);
  const minY = Math.min(y1, y2, y3);
  const maxY = Math.max(y1, y2, y3);

  const scale = Math.min(
    (canvas.width - 2 * margin) / (maxX - minX || 1),
    (canvas.height - 2 * margin) / (maxY - minY || 1)
  );

  const px = [
    margin + (x1 - minX) * scale,
    margin + (x2 - minX) * scale,
    margin + (x3 - minX) * scale
  ];
  const py = [
    canvas.height - (margin + (y1 - minY) * scale),
    canvas.height - (margin + (y2 - minY) * scale),
    canvas.height - (margin + (y3 - minY) * scale)
  ];

  // Draw triangle lines
  ctx.strokeStyle = "blue";
  ctx.lineWidth = 3;
  ctx.beginPath();
  ctx.moveTo(px[0], py[0]);
  ctx.lineTo(px[1], py[1]);
  ctx.lineTo(px[2], py[2]);
  ctx.closePath();
  ctx.stroke();

  // Draw accurate arcs and angle values
  for (let i = 0; i < 3; i++) {
  const a = i, b = (i + 1) % 3, c = (i + 2) % 3;
  const AB = {x: px[b] - px[a], y: py[b] - py[a]};
  const AC = {x: px[c] - px[a], y: py[c] - py[a]};

  // Compute internal angle using dot and cross products
  const dot = AB.x * AC.x + AB.y * AC.y;
  const cross = AB.x * AC.y - AB.y * AC.x;
  const angle = Math.atan2(Math.abs(cross), dot);
  const angleDeg = (angle * 180 / Math.PI).toFixed(1);

  // Draw arc that touches both triangle sides
  const radius = 40;
  let angleAB = Math.atan2(AB.y, AB.x);
  let angleAC = Math.atan2(AC.y, AC.x);
  // Ensure arc goes in the correct direction (counterclockwise)
    let startAngle = angleAC;
    let endAngle = angleAB;
    let sweep = endAngle - startAngle;
    if (sweep < 0) sweep += 2 * Math.PI;
    // אם הסיבוב גדול מ-π, נצייר בכיוון ההפוך
    if (sweep > Math.PI) {
      startAngle = angleAB;
      endAngle = angleAC;
      sweep = endAngle - startAngle;
      if (sweep < 0) sweep += 2 * Math.PI;
    }
    ctx.beginPath();
    ctx.strokeStyle = "red";
    ctx.lineWidth = 2;
    ctx.arc(px[a], py[a], radius, startAngle, endAngle, false);
    ctx.stroke();

    // Draw angle text
    const midAngle = startAngle + sweep / 2;
    const labelRadius = radius + 20;
    const lx = px[a] + labelRadius * Math.cos(midAngle);
    const ly = py[a] + labelRadius * Math.sin(midAngle);
    ctx.fillStyle = "black";
    ctx.font = "18px Arial";
    ctx.fillText(angleDeg + "°", lx - 15, ly + 7);
  }
}
