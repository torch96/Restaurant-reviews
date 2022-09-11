import React, { useState } from "react";
import loginDataService from "../services/loginAuth.js";
import { useHistory} from "react-router-dom";
import "../index.css";

const Login = props => {
  const history = useHistory()
  const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')

	async function loginUser(event) {
		event.preventDefault()
    console.log(email, password);
		const login = loginDataService.login(
      email,
      password)
      .then(response => {

        console.log(response.data);        
        history.push("/" );
        history.go(0);
      }
      ).catch(e => {
        console.log(e);
      }
      );
      console.log(login);
      
	}



  return (
    <div className="submit-form">
      <div>
        <div className="form-group">
          <label htmlFor="email">Username</label>
          <input
            type="text"
            className="form-control"
            id="email"
            required
            value={email}
            onChange={e => setEmail(e.target.value.toLowerCase())}
            name="email"
          />
        </div>

        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="text"
            className="form-control"
            id="password"
            required
            value={password}
            onChange={e => setPassword(e.target.value)}
            name="password"
          />
        </div>

        <button onClick={loginUser} className="btn btn-success">
          Login
        </button>
      </div>
    </div>
  );
};

export default Login;