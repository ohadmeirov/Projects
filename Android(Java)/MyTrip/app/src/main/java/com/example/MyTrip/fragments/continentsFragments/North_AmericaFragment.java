package com.example.MyTrip.fragments.continentsFragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;

import com.example.MyTrip.R;
import com.example.MyTrip.fragments.countriesFragments.CountriesAdapter;

import java.util.ArrayList;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link North_AmericaFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class North_AmericaFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private RecyclerView recyclerViewCountries;

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public North_AmericaFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment North_AmericaFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static North_AmericaFragment newInstance(String param1, String param2) {
        North_AmericaFragment fragment = new North_AmericaFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the fragment layout
        View view = inflater.inflate(R.layout.fragment_north__america, container, false);

        // Initialize RecyclerView
        recyclerViewCountries = view.findViewById(R.id.recyclerViewNorthAmerica);
        recyclerViewCountries.setLayoutManager(new LinearLayoutManager(getContext()));

        // Set the adapter
        CountriesAdapter adapter = new CountriesAdapter(getNorthAmericaCountries());
        recyclerViewCountries.setAdapter(adapter);
        // Initialize SearchBox
        EditText searchBox = view.findViewById(R.id.editTextSearchNorthAmerica);
        searchBox.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {
            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                // Filter adapter
                if (adapter != null) {
                    adapter.getFilter().filter(s);
                }
            }

            @Override
            public void afterTextChanged(Editable s) {
            }
        });
        return view;
    }

    private List<String> getNorthAmericaCountries() {
        List<String> countries = new ArrayList<>();
        countries.add("United States");
        countries.add("Canada");
        countries.add("Mexico");
        countries.add("Guatemala");
        countries.add("Honduras");
        countries.add("Cuba");
        countries.add("Dominican Republic");
        countries.add("El Salvador");
        countries.add("Panama");
        countries.add("Costa Rica");
        countries.add("Jamaica");
        countries.add("Trinidad and Tobago");
        countries.add("Belize");
        countries.add("Barbados");
        return countries;
    }
}
