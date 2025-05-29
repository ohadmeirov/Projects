import React, { useState, useEffect } from 'react';
import {
    Container,
    Grid,
    TextField,
    Button,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Typography,
    Box,
    Card,
    CardContent,
    Rating,
    Divider
} from '@mui/material';
import { Search as SearchIcon, LocationOn } from '@mui/icons-material';
import axios from 'axios';

const AdvancedSearch = () => {
    const [searchParams, setSearchParams] = useState({
        query: '',
        country: '',
        minRating: '',
        maxRating: ''
    });
    const [countries, setCountries] = useState([]);
    const [results, setResults] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchCountries();
    }, []);

    const fetchCountries = async () => {
        try {
            const response = await axios.get('http://localhost:5000/api/countries');
            setCountries(response.data);
        } catch (error) {
            console.error('Error fetching countries:', error);
            setError('Failed to load countries');
        }
    };

    const handleSearch = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('http://localhost:5000/api/posts/search', {
                params: searchParams,
                headers: { Authorization: `Bearer ${token}` }
            });
            setResults(response.data);
        } catch (error) {
            console.error('Error searching posts:', error);
            setError('Failed to search posts');
        }
    };

    const handleKeyPress = (event) => {
        if (event.key === 'Enter') {
            handleSearch();
        }
    };

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            <Typography variant="h4" gutterBottom>
                Advanced Search
            </Typography>

            <Grid container spacing={3} sx={{ mb: 4 }}>
                <Grid item xs={12} md={4}>
                    <TextField
                        fullWidth
                        label="Search Query"
                        value={searchParams.query}
                        onChange={(e) => setSearchParams({ ...searchParams, query: e.target.value })}
                        onKeyPress={handleKeyPress}
                    />
                </Grid>
                <Grid item xs={12} md={3}>
                    <FormControl fullWidth>
                        <InputLabel>Country</InputLabel>
                        <Select
                            value={searchParams.country}
                            label="Country"
                            onChange={(e) => setSearchParams({ ...searchParams, country: e.target.value })}
                            onKeyPress={handleKeyPress}
                        >
                            <MenuItem value="">All Countries</MenuItem>
                            {countries.map((country) => (
                                <MenuItem key={country._id} value={country._id}>
                                    {country.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Grid>
                <Grid item xs={12} md={2}>
                    <TextField
                        fullWidth
                        label="Min Rating"
                        type="number"
                        value={searchParams.minRating}
                        onChange={(e) => setSearchParams({ ...searchParams, minRating: e.target.value })}
                        onKeyPress={handleKeyPress}
                        InputProps={{ inputProps: { min: 0, max: 10 } }}
                    />
                </Grid>
                <Grid item xs={12} md={2}>
                    <TextField
                        fullWidth
                        label="Max Rating"
                        type="number"
                        value={searchParams.maxRating}
                        onChange={(e) => setSearchParams({ ...searchParams, maxRating: e.target.value })}
                        onKeyPress={handleKeyPress}
                        InputProps={{ inputProps: { min: 0, max: 10 } }}
                    />
                </Grid>
                <Grid item xs={12} md={1}>
                    <Button
                        fullWidth
                        variant="contained"
                        color="primary"
                        onClick={handleSearch}
                        sx={{ height: '100%' }}
                    >
                        <SearchIcon />
                    </Button>
                </Grid>
            </Grid>

            <Divider sx={{ mb: 4 }} />

            <Grid container spacing={4}>
                {results.map((post) => (
                    <Grid item key={post._id} xs={12} md={6} lg={4}>
                        <Card
                            sx={{
                                height: '100%',
                                display: 'flex',
                                flexDirection: 'column',
                                transition: 'transform 0.2s',
                                '&:hover': {
                                    transform: 'scale(1.02)'
                                }
                            }}
                        >
                            {post.mediaUrl && (
                                <img
                                    src={post.mediaUrl}
                                    alt={post.title}
                                    style={{
                                        width: '100%',
                                        height: 200,
                                        objectFit: 'cover'
                                    }}
                                />
                            )}
                            <CardContent sx={{ flexGrow: 1 }}>
                                <Typography gutterBottom variant="h5" component="h2">
                                    {post.title}
                                </Typography>
                                <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                    <LocationOn color="action" sx={{ mr: 1 }} />
                                    <Typography variant="body2" color="text.secondary">
                                        {post.country?.name}
                                    </Typography>
                                </Box>
                                <Typography variant="body2" color="text.secondary" paragraph>
                                    {post.content.length > 150
                                        ? `${post.content.substring(0, 150)}...`
                                        : post.content}
                                </Typography>
                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                    <Rating value={post.rating} readOnly size="small" />
                                    <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                                        ({post.rating}/10)
                                    </Typography>
                                </Box>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>

            {error && (
                <Typography color="error" sx={{ mt: 2 }}>
                    {error}
                </Typography>
            )}
        </Container>
    );
};

export default AdvancedSearch;
