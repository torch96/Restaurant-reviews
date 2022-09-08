# Restaurant-reviews
The main goal of this application is to help users find local restaurants and review them. Once a user creates an account, they will be able to leave a review and then be able to update or delete it.



# api endpoints
#### Restaurant

- `GET /api/v1/restaurants/search/:query` - searchs all restuarants containing query string.
- `GET /api/v1/restaurants/id/:id` - returns restaurant by id.
- `POST /api/v1/restaurants/reviews` - Creates a review.
- `PUT /api/v1/restaurants/reviews` - Updates a review.
- `DELETE /api/v1/restaurants/reviews` - Deletes a review.

#### Users
- `POST /api/v1/users/register` - Registers a user and checks that email has not been registrated yet.
- `POST /api/v1/users/signin` - Creates a JSON web token and inserts it into sessions database.
- `POST /api/v1/users/logout` - Removes JSON Web token from sessions database.
