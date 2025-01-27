package com.example.MyTrip.fragments.countriesFragments;

public class CountryAction {
    private final String countryName;
    private final int actionId;

    public CountryAction(String countryName, int actionId) {
        this.countryName = countryName;
        this.actionId = actionId;
    }

    public String getCountryName() {
        return countryName;
    }

    public int getActionId() {
        return actionId;
    }
}

