import http from "../http-common";
const URL = "https://localhost:5000/api/v1/restaurants/";
class RestaurantDataService {
  async getAll(page = 0) {
    console.log(URL + `restaurants`);
    return http.get( URL );
    
  }

  async get(id) {
    console.log(URL + `${id}`);
    console.log(URL + `id/${id}`);
    return http.get(URL + `id/${id}`);
  }

  async find(query, by = "name", page = 0) {
    console.log(URL + `search/${query}`);
    return http.get( URL +`search/${query}`);
  } 

  async createReview(data) {
    return await fetch(URL + 'reviews', {
      method: 'POST',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${data.jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        restaurant_id: data.restaurant_id,
        name: data.name,
        Review: data.text,
      }),
    })
    
  }

  async updateReview(data) {
    return await fetch(URL + 'reviews', {
      method: 'PUT',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${data.jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        review_id: data.review_id,
        name: data.name,
        Review: data.text,
      }),
    })
    
  }

  async deleteReview(id, jwt ) {
    
    return await fetch(URL + 'reviews', {
    method: 'DELETE',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        review_id : id,
      }),
    })
  }

 /* getGenres(id) {
    return http.get(`/genres`);
  }*/

}

export default new RestaurantDataService();
