# TipTrip

TipTrip is a social platform for sharing travel tips and experiences. Users can create posts about their travels, share tips about different countries, and interact with other travelers.

## Features

- User authentication (register, login)
- Create, read, update, and delete posts
- Like and comment on posts
- Follow other users
- Search for posts and countries
- Country information and ratings

## Tech Stack

- Node.js
- Express.js
- MongoDB
- Mongoose
- JWT Authentication

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- MongoDB

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/tiptrip.git
cd tiptrip
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env` file in the root directory with the following variables:
```
PORT=5000
MONGODB_URI=mongodb://localhost:27017/tiptrip
JWT_SECRET=your_jwt_secret_key_here
```

4. Start the development server:
```bash
npm run dev
```

## API Endpoints

### Users
- POST /api/users/register - Register a new user
- POST /api/users/login - Login user
- GET /api/users/me - Get current user
- PUT /api/users/profile - Update user profile
- POST /api/users/follow/:id - Follow a user
- POST /api/users/unfollow/:id - Unfollow a user

### Posts
- GET /api/posts - Get all posts
- GET /api/posts/:id - Get a single post
- POST /api/posts - Create a new post
- PUT /api/posts/:id - Update a post
- DELETE /api/posts/:id - Delete a post
- POST /api/posts/:id/like - Toggle like on a post
- POST /api/posts/:id/comment - Add a comment to a post
- GET /api/posts/search/:query - Search posts

### Countries
- GET /api/countries - Get all countries
- GET /api/countries/:id - Get a single country
- GET /api/countries/search/:query - Search countries

## License

This project is licensed under the MIT License.
