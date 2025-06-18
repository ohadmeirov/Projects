package com.example.placemeui;

import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;

public class HelloController {
    @FXML
    private Label welcomeText;

    @FXML
    private TextArea reservationJsonArea;
    @FXML
    private TextArea cancelJsonArea;
    @FXML
    private TextArea waitlistJsonArea;

    @FXML
    private StackPane rootPane;
    @FXML
    private VBox welcomeScreen;
    @FXML
    private VBox mainScreen;

    @FXML
    protected void onGetStartedClick() {
        welcomeScreen.setVisible(false);
        mainScreen.setVisible(true);
    }

    @FXML
    protected void onReserveButtonClick() {
        try {
            String jsonBody = reservationJsonArea.getText();
            String response = HelloApplication.sendPostRequestStatic("reservation/reserve", jsonBody);
            welcomeText.setText("Reserve: " + response);
        } catch (Exception e) {
            welcomeText.setText("Reserve Error: " + e.getMessage());
        }
    }

    @FXML
    protected void onCancelButtonClick() {
        try {
            String jsonBody = cancelJsonArea.getText();
            String response = HelloApplication.sendPostRequestStatic("reservation/cancel", jsonBody);
            welcomeText.setText("Cancel: " + response);
        } catch (Exception e) {
            welcomeText.setText("Cancel Error: " + e.getMessage());
        }
    }

    @FXML
    protected void onWaitlistButtonClick() {
        try {
            String jsonBody = waitlistJsonArea.getText();
            String response = HelloApplication.sendPostRequestStatic("reservation/waitlist", jsonBody);
            welcomeText.setText("Waitlist: " + response);
        } catch (Exception e) {
            welcomeText.setText("Waitlist Error: " + e.getMessage());
        }
    }
}
