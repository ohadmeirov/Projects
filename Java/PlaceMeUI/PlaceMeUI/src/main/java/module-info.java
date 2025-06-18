module com.example.placemeui {
    requires javafx.controls;
    requires javafx.fxml;


    opens com.example.placemeui to javafx.fxml;
    exports com.example.placemeui;
}