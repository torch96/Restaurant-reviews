import React, { useEffect } from "react";
import { useHistory, Switch, Route } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";
import AddReview from "./components/add-review";
import Restaurant from "./components/restaurants";
import RestaurantsList from "./components/restaurants-list";
import Login from "./components/login";
import SignUp from "./components/register";
import loginDataService from "./services/loginAuth";

function App() {
  const [user, setUser] = React.useState(false);
  const history = useHistory();
  useEffect(() => {
    isLoggedIn()
  }, []);

  async function isLoggedIn() {
    if (loginDataService.getUser() != null) {

      setUser(true);
    } else {
      setUser(null);
    }


  }

  async function logout() {

    const x = await loginDataService.logout(localStorage.getItem("jwt"))
      .then(response => {
        history.push("/");
        history.go(0);
        localStorage.removeItem("jwt");
      }
      ).catch(e => {
        console.log(e);
      }
      );
    localStorage.removeItem("jwt");
    setUser(null)
  }

  //window.addEventListener('load', login());


  return (

    <div>



      <header className="fixed-top ">
        <nav className="navbar navbar-expand-md fixed-top ">
          <div className="container-fluid">
            <div className="navbar-brand nav-item">

              <a className="navbar-brand" href="/restaurants">New York Restaurants</a>
            </div>


            <div className="collapse navbar-collapse" id="navbarCollapse">
              <ul className="navbar-nav me-auto mb-2 mb-md-0">

                <li className="nav-item menuItem">
                  <a className="nav-link" href="/restaurants">Search
                  </a>
                </li>

                {(localStorage.getItem("jwt") != null) ? (
                  <li className="nav-item" >
                    <a href="/" onClick={logout} className="nav-link" style={{ cursor: 'pointer' }}>
                      Logout
                    </a>
                  </li>
                ) : (
                  <><li className="nav-item menuItem">
                    <a className="nav-link" href="/users/login">Login
                    </a>
                  </li><li className="nav-item menuItem">
                      <a className="nav-link" href="/users/register">register
                      </a>
                    </li></>
                )}

              </ul>
            </div>
          </div>
        </nav>
      </header>

      <div>



        <div className="container-lg  main " >
          <Switch>


            <Route exact path={["/", "/index.html", "/restaurants"]} component={RestaurantsList} />

            <Route
              path="/restaurants/:id/review"
              render={(props) => (
                <AddReview {...props} user={user} />
              )}
            />
            <Route
              path="/restaurants/:id"
              render={(props) => (
                <Restaurant {...props} user={user} />
              )}
            />
            <Route
              path="/users/login"
              render={(props) => (
                <Login {...props} login={Login} />
              )}
            />
            <Route
              path="/users/register"
              render={(props) => (
                <SignUp {...props} SignUp={SignUp} />
              )}
            />
          </Switch>
        </div>


        <footer className="container">
          <p className="float-end"><a href="/">Back to top</a></p>
          <p>© 2017–2021 Company, Inc. · <a href="/">Privacy</a> · <a href="/">Terms</a></p>
        </footer>
      </div></div>
  );
}

export default App;
