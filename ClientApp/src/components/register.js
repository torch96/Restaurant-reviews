import React, { useState } from "react";

import loginDataService from "../services/loginAuth.js";

const SignUp = props => {
    
	const [name, setName] = useState('')
	const [email, setEmail] = useState('')
    const [email2, setEmail2] = useState('')
	const [password, setPassword] = useState('')
    const [password2, setPassword2] = useState('')
	async function registerUser(event) {
		event.preventDefault()
        if(email === email2 && password === password2){
        loginDataService.register(
            name,
            email, 
            password)
            .then(response => {
                console.log(response.data);
                props.history.push("/");
                props.history.go(0);
            }).catch(e => {
                console.log(e);
            }
            );
        }else if(email !== email2){
            alert("Emails do not match")
        }else if(password !== password2){
            alert("Passwords do not match")
        }
		
	}

    return (
        <div className="submit-form">
            <div>
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
                    <label htmlFor="email2"> confirm email</label>
                    <input
                        type="text"
                        className="form-control"
                        id="email2"
                        required
                        value={email2}
                        onChange={(e) => setEmail2(e.target.value.toLowerCase())}
                        name="email2"
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
                <div className="form-group">
                    <label htmlFor="password2"> Confirm Password</label>
                    <input
                        type="text"
                        className="form-control"
                        id="password2"
                        required
                        value={password2}
                        onChange={(e) => setPassword2(e.target.value)}
                        name="password2"
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