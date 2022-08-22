import http from "../http-common";
const URL = "https://localhost:5000/api/v1/";
class RestaurantDataService {
  async getAll(page = 0) {
    console.log(URL + `restaurants?page=${page}`);
    return await fetch( URL + `restaurants`, {
      method: 'get',
      mode: "cors",
    })
    .then(response => response.json())
      
    
  }

  async get(id) {
    
    return http.get(`/id/${id}`);
  }

  async find(query, by = "name", page = 0) {
    console.log(URL + `restaurants?${by}=${query}&page=${page}`);
    return http.get( `restaurants?${by}=${query}&page=${page}`);
  } 

  async createReview(data) {
    return await fetch(URL + '/reviews', {
      method: 'POST',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${data.jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        restaurant_id: data.restaurant_id,
        name: data.name,
        text: data.text,
      }),
    })
    
  }

  async updateReview(data) {
    return await fetch(URL + '/reviews', {
      method: 'PUT',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${data.jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        review_id: data.review_id,
        name: data.name,
        text: data.text,
      }),
    })
    
  }

  async deleteReview(id, jwt ) {
    
    return await fetch(URL + '/reviews', {
    method: 'DELETE',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${jwt}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        comment_id : id,
      }),
    })
  }

 /* getGenres(id) {
    return http.get(`/genres`);
  }*/

}

export default new RestaurantDataService();
