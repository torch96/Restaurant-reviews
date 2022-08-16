import React, { useState, useEffect } from "react";
import MovieDataService from "../services/restaurant";
import loginDataService from "../services/loginAuth";
import { Link } from "react-router-dom";

const Movie = props => {
  const initialMovieState = {
    id: null,
    title: "",
    poster: "",
    fullplot: "",
    imdb: {},
    genres: [],
    cast:[],
    directors:[],
    writers:[],
    num_mflix_comments: 0,
    comments: []
  };
  const initialUserState = {
    name: "",
    email: "",
    password: "",
  };
  const [movie, setMovie] = useState(initialMovieState);
  const [user, setUser] = useState(initialUserState);
  const getMovie = id => {
    MovieDataService.get(id)
      .then(response => {
        setMovie(response.data);
       
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
    getMovie(props.match.params.id);
    getUser();
    
  }, [props.match.params.id]);

  const deleteReview = (commentsId, index) => {
    MovieDataService.deleteReview(commentsId, loginDataService.getJwt())
      .then(response => {
        setMovie((prevState) => {
          prevState.comments.splice(index, 1)
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
      {movie ? (   
        <div>
          <div className="movieInfo card w-50 border-dark mx-auto d-block">
            <h3>{movie.title}</h3>
            <img src={movie.poster} className="posterBig mx-auto d-block" ></img>
            <div className="card  card-body border-dark ">
              <p><strong>Plot: </strong>{movie.fullplot}<br/></p>

              <p><strong>Directors: </strong>{movie.directors.join(", ")}</p>

              <p><strong>Cast: </strong>{movie.cast.join(", ")}</p>
              
              <p><strong>Genres: </strong>{movie.genres.join(", ")}</p>
            </div>
          </div>
      
          <Link to={"/movies/" + props.match.params.id + "/review"} className="btn btn-primary">
            Add Review
          </Link>
          
          <h4> Reviews </h4>
         
          <div className="row">
            {movie.comments.length 
            > 0 ? (
             movie.comments.map((comment, index) => {
               return (
                 <div className="col-lg-4 pb-1" key={index}>
                   <div className="card">
                     <div className="card-body">
                       <p className="card-text">
                         {comment.text}<br/>
                         <strong>User: </strong>{comment.name}<br/>
                         <strong>Date: </strong>{comment.date}
                       </p>
                       {user.email === comment.email &&
                          <div className="row">
                            <a onClick={() => deleteReview(comment._id, index)} className="btn btn-primary col-lg-5 mx-1 mb-1">Delete</a>
                            <Link to={{
                              pathname: "/movies/" + props.match.params.id + "/review",
                              state: {
                                currentReview: comment
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
          <p>No movie selected.</p>
        </div>
      )}
    </div>
  );
};

export default Movie;