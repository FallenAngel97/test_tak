import React from 'react';
import "./LoginPopup.scss";

export default class LoginPopup extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: ''
        }
        this.changeName = this.changeName.bind(this);
        this.changePassword = this.changePassword.bind(this);
        this.submitForm = this.submitForm.bind(this);
    }
    changeName(event) {
        this.setState({
            username: event.target.value
        })
    }
    changePassword(event) {
        this.setState({
            password: event.target.value
        })
    }
    submitForm(event) {
        event.preventDefault();
        let xhr = new XMLHttpRequest();
        let credentialsBody = `username=${encodeURIComponent(document.getElementById('user_login').value)}&password=${encodeURIComponent(document.getElementById('user_password').value)}&grant_type=password`;
        xhr.open('POST', '/token');
        xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xhr.send(credentialsBody);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const token = JSON.parse(xhr.responseText)['access_token']
                localStorage.setItem('token', token);
                this.props.setUserToken(token);
            }
        }
    }
    render() {
        return (
            <aside id='modal_background'>
                <form id='login_form' onSubmit={this.submitForm}>
                    <div className='form-group'>
                        <label htmlFor="username">Username</label>
                        <input id='user_login' className="form-control" type='text' name='username' />
                    </div>
                    <div className="form-group">
                        <label htmlFor='password'>Password</label>
                        <input id='user_password' className="form-control" type='password' name='password' />
                    </div>
                    <input className="btn btn-primary" type='submit' value='Login' />
                </form>
            </aside>
        )
    }
}