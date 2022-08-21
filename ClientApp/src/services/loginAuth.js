//import Axios from "../Axios-common";
import {useHistory} from "react-router-dom";
const URL = "http://localhost:5000/api/v1";


class loginDataService {
 async register(name , email, password) {
    const response = await fetch(URL + '/users/register', {
			method: 'POST',
      mode: "cors",
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify({
				name,
				email,
				password,
			}),
		})
    const data = await response.json();
   localStorage.setItem("jwt", data.token);   
   localStorage.setItem("name", data.info.name);
    return response;
  
  }
  async login(email, password) {
    const response = await fetch(URL + '/users/signin', {
      method: 'POST',
      mode: "cors",
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email,
        password,
      }),
    })
    const data = await response.json();
    localStorage.setItem("jwt", data.token);   
    
    
    return response;
    
   
  }

 async logout(jwt) {
    const response = await fetch(URL + '/users/logout', {
      method: 'POST',
      mode: "cors",
      headers: {
        Authorization: `Bearer ${jwt}`,
        'Content-Type': 'application/json',
      },
      
    })
    localStorage.removeItem("jwt");
    
    }


  getJwt() {
    return localStorage.getItem("jwt");
  }
  getName() {
    return localStorage.getItem("name");
  }
  parseJwt() {
    const token = this.getJwt();
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  }

  getUser() {
    try {
     
      if(this.getJwt() !== null) {
        const decoded = this.parseJwt();
        
        return decoded;
      }else {
        return {email: "", name: ""};
      }

    } catch(e) {
      return null;
    }

  }
}
export default new loginDataService();