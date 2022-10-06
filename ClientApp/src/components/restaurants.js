import React, { useState, useEffect, useCallback } from "react";
import RestaurantDataService from "../services/restaurant";
import loginDataService from "../services/loginAuth";
import { Link } from "react-router-dom";

const Restaurant = props => {
  const initialRestaurantState = {
    id: null,
    name: "",
    address: {
      building: "",
      coord:[],
      street: "",
      zipcode: ""

    },
    borough: "",
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
        console.log(response);
        setRestaurant(response);
        console.log(restaurant.address.coord);
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
    
  }, []);

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
/*  function loadMapScenario() {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        /* No need to set credentials if already passed in URL 
        center: new Microsoft.Maps.Location(restaurant.address.coord[1] , restaurant.address.coord[0])
    });
    map.setView({ mapTypeId: Microsoft.Maps.MapTypeId.birdseye, heading: 90 });
  
} */ 
  
  return (
    <div>
      {restaurant ? (   
        <div>
          <div className="movieInfo  mx-auto d-block">
           
            <img src={"https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/" + restaurant.address.coord[1] + "," + restaurant.address.coord[0] +"/16?mapSize=650,700&pp="+ restaurant.address.coord[1] + "," + restaurant.address.coord[0]+"&key=AohnDNk_k1STAWaPrlL114lEdu9SABRTEAsJdSKsC-d020EmFCRwQxOVaf_qCPdM"} alt="" className="posterBig mx-auto d-block " ></img>
            <script type='text/javascript' src='https://www.bing.com/api/maps/mapcontrol?key=AohnDNk_k1STAWaPrlL114lEdu9SABRTEAsJdSKsC-d020EmFCRwQxOVaf_qCPdMy&callback=loadMapScenario' async defer></script>
            
            
            <div className="card card-body border-dark restaurantCard ">
             <h3 className="content">{restaurant.name}</h3>
             <hr/>
              <p className="restaurantBody content"><strong>Cuisine: </strong>{restaurant.cuisine}</p>
            
              <p className="restaurantBody content"><strong>Address: </strong>{restaurant.address.building} {restaurant.address.street}, {restaurant.address.zipcode}</p>
              <p className="restaurantBody content"> <strong>Borough: </strong>{restaurant.borough} </p>
              <a  href={"https://www.bing.com/maps?q=" + restaurant.address.street + restaurant.address.zipcode} className="btnReview btn btn-primary col-lg-5 mx-1 mb-1 btnSize mtd" >Directions</a>
            
            </div>
          </div>
      
        
          
          <h4> Reviews </h4>
         
          <div className="row">
            {restaurant.reviews.length 
            > 0 ? (
             restaurant.reviews.map((review, index) => {
               return (
                 <div className="col-lg-4 pb-1" key={index}>
                   <div className="card">
                     <div className="card-body">
                       <p className="content card-text">
                         {review.text}<br/>
                         <strong>User: </strong>{review.name}<br/>
                         <strong>Date: </strong>{review.date}
                       </p>
                       {user.email === review.email &&
                          <div className="row">
                            <a href={ "/restaurants/" + props.match.params.id + "/review"} onClick={() => deleteReview(review._id, index)} className="btn btn-primary col-lg-5 mx-1 mb-1 content">Delete</a>
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
            
             
             <p > <Link to={"/restaurants/" + props.match.params.id + "/review"} className="btn btn-primary addreview float-right">
            Add Review 
          </Link> </p>
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