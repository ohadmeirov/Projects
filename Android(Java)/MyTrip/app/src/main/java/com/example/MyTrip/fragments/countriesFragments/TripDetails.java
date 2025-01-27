package com.example.MyTrip.fragments.countriesFragments;

public class TripDetails {
    private String titleEn;
    private String titleHe;
    private String descriptionEn;
    private String descriptionHe;
    private String linkUrl;
    private String buttonText;

    public TripDetails(
            String titleEn,
            String titleHe,
            String descriptionEn,
            String descriptionHe,
            String linkUrl,
            String buttonText
    ) {
        this.titleEn = titleEn;
        this.titleHe = titleHe;
        this.descriptionEn = descriptionEn;
        this.descriptionHe = descriptionHe;
        this.linkUrl = linkUrl;
        this.buttonText = buttonText;
    }

    // Add getters for new fields
    public String getTitleEn() { return titleEn; }
    public String getTitleHe() { return titleHe; }
    public String getDescriptionEn() { return descriptionEn; }
    public String getDescriptionHe() { return descriptionHe; }

    // Existing getters remain the same
    public String getLinkUrl() { return linkUrl; }
    public String getButtonText() { return buttonText != null ? buttonText : "Learn More Online"; }
}