import axios from "axios";

export default axios.create({
  baseURL: "http://:5000/api/v1/",
  header:{
    "Content-type": "application/json"
  }
});
