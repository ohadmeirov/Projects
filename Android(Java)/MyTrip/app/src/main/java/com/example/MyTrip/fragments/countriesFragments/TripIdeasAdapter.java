package com.example.MyTrip.fragments.countriesFragments;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.LinearLayout;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.example.MyTrip.R;

import java.util.ArrayList;
import java.util.List;

public class TripIdeasAdapter extends RecyclerView.Adapter<TripIdeasAdapter.ViewHolder> {
    private final List<TripDetails> tripDetailsList;
    private final LinearLayout mainContainer;
    private boolean isEnglish = true;

    public TripIdeasAdapter(List<TripDetails> tripDetailsList, LinearLayout mainContainer) {
        this.tripDetailsList = tripDetailsList;
        this.mainContainer = mainContainer;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_trip_idea, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        TripDetails tripDetail = tripDetailsList.get(position);
        holder.textViewIdea.setText(tripDetail.getTitleEn());

        holder.buttonDetails.setOnClickListener(v -> {
            for (int i = 0; i < mainContainer.getChildCount(); i++) {
                mainContainer.getChildAt(i).setVisibility(View.GONE);
            }

            View detailView = LayoutInflater.from(v.getContext())
                    .inflate(R.layout.layout_trip_details, mainContainer, false);

            TextView titleText = detailView.findViewById(R.id.textViewDetailTitle);
            TextView descriptionText = detailView.findViewById(R.id.textViewDetailDescription);
            Button backButton = detailView.findViewById(R.id.buttonBack);
            Button searchButton = detailView.findViewById(R.id.buttonSearch);
            Button languageToggleButton = detailView.findViewById(R.id.buttonLanguageToggle);

            // Initial setup in English
            titleText.setText(tripDetail.getTitleEn());
            descriptionText.setText(tripDetail.getDescriptionEn());
            languageToggleButton.setText("עברית");
            isEnglish = true;

            // Language toggle functionality
            languageToggleButton.setOnClickListener(langView -> {
                if (isEnglish) {
                    // Switch to Hebrew
                    titleText.setText(tripDetail.getTitleHe());
                    descriptionText.setText(tripDetail.getDescriptionHe());
                    languageToggleButton.setText("English");
                    isEnglish = false;
                } else {
                    // Switch to English
                    titleText.setText(tripDetail.getTitleEn());
                    descriptionText.setText(tripDetail.getDescriptionEn());
                    languageToggleButton.setText("עברית");
                    isEnglish = true;
                }
            });

            if (tripDetail.getLinkUrl() != null && !tripDetail.getLinkUrl().isEmpty()) {
                searchButton.setVisibility(View.VISIBLE);
                searchButton.setText(tripDetail.getButtonText());
                searchButton.setOnClickListener(searchView -> {
                    Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(tripDetail.getLinkUrl()));
                    v.getContext().startActivity(browserIntent);
                });
            } else {
                searchButton.setVisibility(View.GONE);
            }

            backButton.setOnClickListener(backView -> {
                mainContainer.removeView(detailView);
                for (int i = 0; i < mainContainer.getChildCount(); i++) {
                    mainContainer.getChildAt(i).setVisibility(View.VISIBLE);
                }
            });

            mainContainer.addView(detailView);
        });
    }

    @Override
    public int getItemCount() {
        return tripDetailsList.size();
    }

    static class ViewHolder extends RecyclerView.ViewHolder {
        TextView textViewIdea;
        Button buttonDetails;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            textViewIdea = itemView.findViewById(R.id.textViewIdea);
            buttonDetails = itemView.findViewById(R.id.buttonLink);
        }
    }
}