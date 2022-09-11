import http from "../http-common";
const URL = "/api/v1/restaurants/";
class RestaurantDataService {
  async getAll(page = 0) {
    console.log(URL + `restaurants`);
    

   const response = await fetch(URL , {
      method: 'GET',
      mode: "cors",
      headers: {
        
        'Content-Type': 'application/json',
      },
    
    })

    return response.json();
  }

  async get(id) {

    const response = await fetch(URL +`id/${id}` , {
      method: 'GET',
      mode: "cors",
      headers: {
        
        'Content-Type': 'application/json',
      },
    
    })

    return await response.json();
  
  }

  async find(query, by = "name", page = 0) {
    console.log(URL + `search/${query}`);
    const response = await fetch(URL + `search/${query}`, {
      method: 'GET',
      mode: "cors",
      headers: {
        
        'Content-Type': 'application/json',
      },
    
    })

    return await response.json();
    
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
