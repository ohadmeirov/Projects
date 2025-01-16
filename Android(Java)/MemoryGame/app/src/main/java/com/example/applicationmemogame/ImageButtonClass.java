package com.example.applicationmemogame;
public class ImageButtonClass {

    int id;
    int arrId;

    ImageButtonClass(int id , int arrId)
    {
        this.arrId = arrId;
        this.id = id;
    }

    public int getId() {
        return id;
    }

    public int getArrId() {
        return arrId;
    }
}
