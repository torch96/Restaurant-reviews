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

  const onChangeSearchTitle = e => {
    const searchTitle = e.target.value;
    setSearchTitle(searchTitle);
  };

  

  

  const retrieveRestaurants = () => {
    RestaurantDataService.getAll()
      .then(response => {
        
        setRestaurants(response.data.restaurants);
        
      })
      .catch(e => {
        console.log(e);
      });
  };

 /* const onChangeSearchGenre = e => {
    const searchGenres = e.target.value;
    setSearchGenre(searchGenres);
    
  };

  const retrieveGenres = () => {
    RestaurantDataService.getGenres()
      .then(response => {
        console.log(response.data);
        setGenres(["All Genres"].concat(response.data));
        
      })
      .catch(e => {
        console.log(e);
      });
  };*/

  const refreshList = () => {
    retrieveRestaurants();
  };

  const find = (query, by) => {
    RestaurantDataService.find(query, by)
      .then(response => {
        console.log(response.data);
        setRestaurants(response.data.restaurants);
      })
      .catch(e => {
        console.log(e);
      });
  };

  const findByTitle = () => {
    find(searchTitle, "title")
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
      findByTitle();
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
            onChange={onChangeSearchTitle}
            onKeyPress={handleKeyPress}
          />
          <div className="input-group-append">
            <button
              className="btn btn-outline-secondary"
              type="button"
              onClick={findByTitle}
              
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
          const address = `${restaurant.imdb.rating}`;
          const genre = [""];
          const cast = [""];
         if(restaurant.genres != null){
          Object.keys(restaurant.genres).forEach(key => {
            genre.push(restaurant.genres[key] + ' ');
           });
          }else{
            genre.push("N/A");
          }
          
          if(restaurant.cast != null){
            Object.keys(restaurant.cast).forEach(key => {
              cast.push(restaurant.cast[key] + ', ');
           });
          }else{
            cast.push("N/A");
          }
          return (
            <div className="col-lg-4 pb-1 container-sm ">
              <div className="card movie border-dark ">
                <div className="card-body">
                  <h5 className="card-title">{restaurant.name}</h5>
                  <img src="" className="poster mx-auto d-block" ></img>
                  <p className="card-text">
                    <strong>Plot: </strong>{}<br/>
                    <strong>Year of release: </strong>{}<br/>
                    <strong>Cast: </strong>{cast}<br/>
                    <strong>Genre: </strong>{genre}<br/>
                    
                    <strong>IMDB RAITNG: </strong>{address}
                  </p>
                  <div className="row">
                  <Link to={"/restaurants/"+restaurant._id} className="btn btn-primary col-lg-5 mx-1 mb-1">
                    View Reviews
                  </Link>
                  
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