import React from 'react';

export default class ProductCard extends React.Component {
    constructor(props) {
        super(props);
        this.changeQuantity = this.changeQuantity.bind(this);
        this.addToOrder = this.addToOrder.bind(this);
        this.state = {
            productQuantity: 1
        }
    }

    changeQuantity(event) {
        let productQuantity = event.target.value;
        this.setState({ productQuantity });
    }

    addToOrder() {
        const token = localStorage.getItem('token');
        if (token == undefined) {
            alert('You need to be logined');
            return;
        }
        var xhr = new XMLHttpRequest();
        let order_id = 1;
        const { productName, price } = this.props.product;
        let order = {};
        order.order_id = order_id;
        order.product_name = productName;
        order.price = price;
        order.quantity = this.state.productQuantity;
        xhr.open("POST", "/api/order/add_to_order");
        xhr.setRequestHeader("Content-type", "text/html");
        xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const productId = parseInt(xhr.responseText);
                if (productId != -1) {
                    this.props.productAdded(order);
                }
            }
        }
        xhr.send(JSON.stringify(order));
    }

    render() {
        const { productName, price } = this.props.product;
        return (
            <div className="card" style={{ width: "18rem", display: 'inline-block', margin: '5px' }}>
                <div className="card-body">
                    <div className='form-row'>
                        <h5 className="card-title form-group col-md-6">{productName}</h5>
                        <span className="form-group col-md-6">{price + '$'}</span>
                    </div>
                    <label htmlFor='product_quantity'>Quantity</label>
                    <input className="form-control" value={this.state.productQuantity} style={{ width: '45px' }} name='product_quantity' onChange={this.changeQuantity} type='text' placeholder='1' title='Quantity' />
                    <br />
                    <a href="#" onClick={this.addToOrder} className="card-link">Add to cart</a>
                </div>
            </div>
        )
    }
}