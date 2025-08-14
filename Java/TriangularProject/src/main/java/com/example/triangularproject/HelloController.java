package com.example.triangularproject;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class HelloController {

    @FXML
    private TextField x1Field, y1Field, x2Field, y2Field, x3Field, y3Field;

    @FXML
    protected void onShowTriangleButtonClick() {
        try {
            double x1 = Double.parseDouble(x1Field.getText());
            double y1 = Double.parseDouble(y1Field.getText());
            double x2 = Double.parseDouble(x2Field.getText());
            double y2 = Double.parseDouble(y2Field.getText());
            double x3 = Double.parseDouble(x3Field.getText());
            double y3 = Double.parseDouble(y3Field.getText());

            FXMLLoader loader = new FXMLLoader(getClass().getResource("triangle-view.fxml"));
            Parent root = loader.load();
            TriangleController controller = loader.getController();
            controller.setPoints(x1, y1, x2, y2, x3, y3);

            Stage stage = (Stage) x1Field.getScene().getWindow();
            stage.setScene(new Scene(root, 800, 800));
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}