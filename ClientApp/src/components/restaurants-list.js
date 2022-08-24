import React, { useState, useEffect } from "react";
import RestaurantDataService from "../services/restaurant";
import { Link } from "react-router-dom";
import "../index.css";
const RestaurantsList = props => {
  const [restaurants, setRestaurants] = useState([]);
  const [searchTitle, setSearchTitle ] = useState("");
  const [searchGenres, setSearchGenre ] = useState("");
  const [genres, setGenres] = useState(["All Genres"]);

  useEffect(() => {
    retrieveRestaurants();
   // retrieveGenres();
  }, []);

  const onChangeSearchName = e => {
    const searchTitle = e.target.value;
    setSearchTitle(searchTitle);
  };

  

  

  const retrieveRestaurants = () => {
    RestaurantDataService.getAll()
      .then(response => {
        console.log(response);
        console.log(localStorage.getItem("jwt"));
        console.log(response.data.Restaurants);
        setRestaurants(response.data.Restaurants);
        
      })
      .catch(e => {
        console.log(e);
      });
      
  };



  const refreshList = () => {
    retrieveRestaurants();
  };

  const find = (query, by) => {
    RestaurantDataService.find(query, by)
      .then(response => {
        console.log(response.data.Restaurants);
        setRestaurants(response.data.Restaurants);
      })
      .catch(e => {
        console.log(e);
      });
  };

  const findByName = () => {
    find(searchTitle, "name");
  };
  

  const findByGenre = () => {
    if (searchGenres == "All Genres") {
      refreshList();
    } else {
      find(searchGenres, "genres")
    }
  };

  const handleKeyPress = (event) => {
    
    if(event.key === 'Enter'){
      findByName();
    }
  }

  return (
    <div>
      <div className="row pb-1">
        <div className="input-group col-lg-4">
          <input
            type="text"
            className="form-control"
            placeholder="Search by Title"
            value={searchTitle}
            onChange={onChangeSearchName}
            onKeyPress={handleKeyPress}
          />
          <div className="input-group-append">
            <button
              className="btn btn-outline-secondary"
              type="button"
              onClick={findByName}
              
            >
              Search
            </button>
          </div>
        </div>
        
        <div className="input-group col-lg-4">

      

        </div>
      </div>
      <div className="row">
        {restaurants.map((restaurant) => {
         const address = `${restaurant.address.building} ${restaurant.address.street}`;

          return (
            <div className="col-lg-4 pb-1 container-sm ">
              <div className="card movie border-dark ">
                <div className="card-body">
                  <h5 className="card-title">{restaurant.name}</h5>
                  <img src="" className="poster mx-auto d-block" ></img>
                  <p className="card-text">
                    <strong>Cuisine: </strong>{restaurant.cuisine}<br/>
                    <strong>Address: </strong>{address}
                    <strong>zipcode: </strong>{restaurant.address.zipcode}<br/>
                  </p>
                  <div className="row">
                  <Link to={"/restaurants/"+restaurant._id} className="btn btn-primary col-lg-5 mx-1 mb-1">
                    View Reviews
                  </Link>
                  <a target="_blank" href={"https://www.google.com/maps/place/" + address+ ","+restaurant.address.zipcode} className="btn btn-primary col-lg-5 mx-1 mb-1">View Map</a>
                  
                  </div>
                </div>
              </div>
            </div>
          );
        })}


      </div>
    </div>
  );
};

export default RestaurantsList;