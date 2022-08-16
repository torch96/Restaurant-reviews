import React, { useState, useEffect } from "react";
import { Link,useHistory } from "react-router-dom";
import loginDataService from "../services/loginAuth.js";

const SignUp = props => {
    const history = useHistory()

	const [name, setName] = useState('')
	const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')

	async function registerUser(event) {
		event.preventDefault()
       const result = loginDataService.register(
            name,
            email, 
            password)
            .then(response => {
                console.log(response.data);
                props.history.push("/");
            }).catch(e => {
                console.log(e);
            }
            );
            console.log(result.json());
		
	}

    return (
        <div className="submit-form">
            <div>
                <div className="form-group">
                    <label htmlFor="email">email</label>
                    <input
                        type="text"
                        className="form-control"
                        id="email"
                        required
                        value={email}
                        onChange={(e) => setEmail(e.target.value.toLowerCase())}
                        name="email"
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="name">Username</label>
                    <input
                        type="text"
                        className="form-control"
                        id="name"
                        required
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        name="name"
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
                        onChange={(e) => setPassword(e.target.value)}
                        name="password"
                    />
                </div>

                <button onClick={registerUser} className="btn btn-success">
                    Sign Up
                </button>
            </div>
        </div>
    );
}
export default SignUp;