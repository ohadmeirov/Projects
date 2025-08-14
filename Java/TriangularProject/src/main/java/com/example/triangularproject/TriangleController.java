package com.example.triangularproject;

import javafx.fxml.FXML;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;

public class TriangleController {

    @FXML
    private Canvas canvas;

    private final double[] xs = new double[3];
    private final double[] ys = new double[3];

    // Store the coordinates of the three points and trigger drawing
    public void setPoints(double x1, double y1, double x2, double y2, double x3, double y3) {
        xs[0] = x1;
        ys[0] = y1;
        xs[1] = x2;
        ys[1] = y2;
        xs[2] = x3;
        ys[2] = y3;
        drawTriangle();
    }

    private void drawTriangle() {
        GraphicsContext gc = canvas.getGraphicsContext2D();
        gc.clearRect(0, 0, canvas.getWidth(), canvas.getHeight());

        // Determine scaling to fit triangle into canvas with margins
        double margin = 80;
        double minX = Math.min(xs[0], Math.min(xs[1], xs[2]));
        double minY = Math.min(ys[0], Math.min(ys[1], ys[2]));
        double maxX = Math.max(xs[0], Math.max(xs[1], xs[2]));
        double maxY = Math.max(ys[0], Math.max(ys[1], ys[2]));

        double scale = Math.min(
                (canvas.getWidth() - 2 * margin) / (maxX - minX),
                (canvas.getHeight() - 2 * margin) / (maxY - minY)
        );

        // Convert original coordinates to scaled pixel positions
        double[] px = new double[3];
        double[] py = new double[3];
        for (int i = 0; i < 3; i++) {
            px[i] = margin + (xs[i] - minX) * scale;
            py[i] = canvas.getHeight() - (margin + (ys[i] - minY) * scale); // invert Y-axis
        }

        // Draw triangle outline
        gc.setStroke(Color.BLUE);
        gc.setLineWidth(3);
        gc.strokePolygon(px, py, 3);

        // Draw arcs for each angle and show angle values
        for (int i = 0; i < 3; i++) {
            int a = i, b = (i + 1) % 3, c = (i + 2) % 3;

            double abx = px[b] - px[a], aby = py[b] - py[a];
            double acx = px[c] - px[a], acy = py[c] - py[a];

            // Calculate angle at vertex a using cross and dot product
            double dot = abx * acx + aby * acy;
            double cross = abx * acy - aby * acx;
            double angleDeg = Math.toDegrees(Math.atan2(Math.abs(cross), dot));

            // Prepare arc position and sweep
            double arcRadius = 40;
            double dirAB = Math.atan2(aby, abx);
            double dirAC = Math.atan2(acy, acx);

            // Convert from mathematical coordinates (atan2) to JavaFX arc coordinates
            double startDeg = 360 - Math.toDegrees(dirAB);
            double endDeg = 360 - Math.toDegrees(dirAC);
            double sweepDeg = endDeg - startDeg;

            // Normalize to positive sweep
            if (sweepDeg < 0) sweepDeg += 360;

            // Always take the smaller sweep (internal angle)
            if (sweepDeg > 180) sweepDeg = 360 - sweepDeg;

            // Draw the arc (negative sweep for correct direction in JavaFX)
            gc.setStroke(Color.RED);
            gc.setLineWidth(2);
            gc.strokeArc(
                    px[a] - arcRadius, py[a] - arcRadius,
                    arcRadius * 2, arcRadius * 2,
                    startDeg, -sweepDeg, javafx.scene.shape.ArcType.OPEN
            );


            // Draw the angle value near the arc
            double midAngle = Math.toRadians(startDeg + sweepDeg / 2);
            double labelRadius = arcRadius + 20;
            double lx = px[a] + labelRadius * Math.cos(midAngle);
            double ly = py[a] + labelRadius * Math.sin(midAngle);

            gc.setFill(Color.BLACK);
            gc.setFont(new Font(18));
            gc.fillText(String.format("%.1f°", angleDeg), lx - 15, ly + 7);
        }
    }
}
