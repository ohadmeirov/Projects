module com.example.triangularproject {
    requires javafx.controls;
    requires javafx.fxml;


    opens com.example.triangularproject to javafx.fxml;
    exports com.example.triangularproject;
}