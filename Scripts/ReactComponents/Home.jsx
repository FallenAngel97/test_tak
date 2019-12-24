import React from 'react';
import ProductCard from './ProductCard.jsx';
import LoginPopup from "./LoginPopup";

class OrdersPanel extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            totalSum: 0
        }
        this.removeFromOrder = this.removeFromOrder.bind(this);
        if (this.props.orders.length != 0) {
            this.calculateTotalPrice();
        }
    }

    UNSAFE_componentWillReceiveProps(nextProps) {
        this.calculateTotalPrice();
    }

    calculateTotalPrice() {
        var xhr = new XMLHttpRequest();
        var order_number = 1;
        xhr.open('GET', '/api/order/get_checkout/' + order_number);
        xhr.setRequestHeader('Authorization', 'Bearer ' + localStorage.getItem('token'));
        xhr.send();
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                this.setState({
                    totalSum: parseFloat(xhr.responseText).toFixed(2)
                })
            }
        }
    }
    removeFromOrder(product_id) {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', '/api/order/remove_order/' + product_id);
        xhr.setRequestHeader('Authorization', 'Bearer ' + localStorage.getItem('token'));
        xhr.send();
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                this.props.refreshProducts(null);
            }
        }
    }
    render() {
        const orders = this.props.orders;
        return (
            <section style={{ border: '.5px solid #d7d7d7', borderRadius: '5px', padding: '5px' }}>
                <span>Your order</span>
                {orders.length == 0 && <div>0 items</div>}
                {orders.map((order) => (
                    <div key={order.id}>{order.productsInCart.map((product) =>
                        <div>
                            <h4>{product.productName} - {product.quantity} pieces</h4>
                            <span>{order.price}</span>
                            <button className='btn' onClick={() => this.removeFromOrder(product.id)}>Remove from order</button>
                        </div>
                    )}</div>
                ))}
                {orders.length != 0 && <div>
                    Total sum: {this.state.totalSum}$
                </div>}
            </section>
        )
    }
}

export default class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            productList: [],
            displayLogin: false,
            userToken: localStorage.getItem('token'),
            userData: undefined
        }
        this.getListOfProducts();
        this.getUserData();
        this.showLoginPopup = this.showLoginPopup.bind(this);
        this.setUserToken = this.setUserToken.bind(this);
        this.productAdded = this.productAdded.bind(this);
    }

    getListOfProducts() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "/api/order");
        xhr.send();
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                this.setState({
                    productList: JSON.parse(xhr.responseText).results
                })
            }
        }
    }

    getUserData() {
        if (this.state.userToken != undefined) {
            let xhr2 = new XMLHttpRequest();
            xhr2.open('GET', '/api/account');
            xhr2.setRequestHeader('Authorization', 'Bearer ' + this.state.userToken)
            xhr2.send();
            xhr2.onreadystatechange = () => {
                if (xhr2.readyState == 4 && xhr2.status == 200) {
                    const userData = JSON.parse(xhr2.responseText).results;
                    this.setState({ userData });
                }
            }
        } 
    }

    showLoginPopup(ev) {
        ev.preventDefault();
        if (this.state.userToken != undefined) {
            this.setState({
                userToken: undefined,
                userData: undefined
            })
            localStorage.removeItem('token');
        } else {
            this.setState({
                displayLogin: true
            })
        }
    }

    setUserToken(userToken) {
        this.setState({
            userToken,
            displayLogin: false
        })
        this.getUserData();
    }

    productAdded(product) {
        this.getListOfProducts();
        this.getUserData();
    }

    render() {
        return (
            <>
                <nav className="navbar navbar-expand-lg navbar-light bg-light row">
                    <div className="col">
                        <a className="navbar-brand" href="#">Sigma Software shop</a>
                    </div>
                    <form onSubmit={this.showLoginPopup} className="form-inline my-2 my-lg-0">
                        <button className="btn btn-outline-success my-2 my-sm-0" type="submit">{this.state.userToken != undefined ? 'Logout' : 'Login'}</button>
                    </form>
                    {this.state.userData != undefined && <span>Discount: {this.state.userData.personalDiscount} %</span>}
                </nav>
                {this.state.userData != undefined && <OrdersPanel refreshProducts={this.productAdded} orders={this.state.userData.orders} />}
                <main>
                    {this.state.productList.map(
                        (product) => <ProductCard productAdded={this.productAdded} key={product.Id} product={product} />
                    )}
                </main>
                {this.state.displayLogin && <LoginPopup setUserToken={this.setUserToken} />}
            </>
        )
    }
}