import React, { useState, useEffect } from "react";
import RestaurantDataService from "../services/restaurant";
import loginDataService from "../services/loginAuth";
import { Link } from "react-router-dom";

const Restaurant = props => {
  const initialRestaurantState = {
    id: null,
    name: "",
    address: {},
    cuisine: "",
    reviews: [""]
  };
  const initialUserState = {
    name: "",
    email: "",
    password: "",
  };
  const [restaurant, setRestaurant] = useState(initialRestaurantState);
  const [user, setUser] = useState(initialUserState);
  const getRestaurant = id => {
    RestaurantDataService.get(id)
      .then(response => {
        console.log(response.data);
        setRestaurant(response.data);
       
      })
      .catch(e => {
        console.log(e,id);
      });
  };
  const getUser = () => {
    loginDataService.getUser()
      setUser(loginDataService.getUser());
    
    
  } 
  
  useEffect(() => {
    getRestaurant(props.match.params.id);
    getUser();
    
  }, [props.match.params.id]);

  const deleteReview = (reviewsId, index) => {
    RestaurantDataService.deleteReview(reviewsId, loginDataService.getJwt())
      .then(response => {
        setRestaurant((prevState) => {
          prevState.reviews.splice(index, 1)
          return({
            ...prevState
          })
        })
      })
      .catch(e => {
        console.log(e);
      });
  };

  return (
    <div>
      {restaurant ? (   
        <div>
          <div className="movieInfo card w-50 border-dark mx-auto d-block">
            <h3>{restaurant.title}</h3>
            <img src={restaurant.poster} className="posterBig mx-auto d-block" ></img>
            <div className="card  card-body border-dark ">
            <p>
            <strong>Cuisine: </strong>{restaurant.cuisine}<br/>
            <strong>Address: </strong>{restaurant.address.building} {restaurant.address.street}, {restaurant.address.zipcode}
          </p>
            </div>
          </div>
      
          <Link to={"/restaurants/" + props.match.params.id + "/review"} className="btn btn-primary">
            Add Review
          </Link>
          
          <h4> Reviews </h4>
         
          <div className="row">
            {restaurant.reviews.length 
            > 0 ? (
             restaurant.reviews.map((review, index) => {
               return (
                 <div className="col-lg-4 pb-1" key={index}>
                   <div className="card">
                     <div className="card-body">
                       <p className="card-text">
                         {review.text}<br/>
                         <strong>User: </strong>{review.name}<br/>
                         <strong>Date: </strong>{review.date}
                       </p>
                       {user.email === review.email &&
                          <div className="row">
                            <a onClick={() => deleteReview(review._id, index)} className="btn btn-primary col-lg-5 mx-1 mb-1">Delete</a>
                            <Link to={{
                              pathname: "/restaurants/" + props.match.params.id + "/review",
                              state: {
                                currentReview: review
                              }
                            }} className="btn btn-primary col-lg-5 mx-1 mb-1">Edit</Link>
                          </div>                   
                       }
                     </div>
                   </div>
                 </div>
               );
             })
             ) : (
              <div className="col-sm-4">
                <p>No reviews yet.</p>
              </div>
              )}
            </div>
          </div>
        ) : (
        <div>
          <br />
          <p>No restaurant selected.</p>
        </div>
      )}
    </div>
  );
};

export default Restaurant;