import React, { useState, useEffect } from 'react';
import { Button, Table } from 'react-bootstrap';
import axios from 'axios';
import ModalEditarPizza from './ModalEditarPizza.js';

async function getPizzaData() {
    // const response = await fetch('pizza');
    const response = await axios.get("api/pizza", {}, {});
    return response.data;
}

async function getSauces() {
    const response = await axios.get("api/pizza/sauces", {}, {});
    // console.log(response.data);
    return response.data;
}
async function getToppings() {
    const response = await axios.get("api/pizza/toppings", {}, {});
    // console.log(response.data);
    return response.data;
}

export function FetchPizzas() {
    const [pizzas, setPizzas] = useState([]);
    const [sauces, setSauces] = useState([]);
    const [toppings, setToppings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [formData, setFormData] = useState({
        id: "",
        name: "",
        operacion: "",
        show: 0
    });

    const reloadData = () => {
        setLoading(true);
        async function getData() {
            const pizzaData = await getPizzaData();
            const sauceData = await getSauces();
            const toppingData = await getToppings();
            setPizzas(pizzaData);
            setSauces(sauceData);
            setToppings(toppingData);
            setLoading(false);
        }
        getData();
    };

    useEffect(() => {
        reloadData();
    }, []);

    const renderPizzaTable = () => (
        <>
            <Button variant="primary" onClick={() => {
                setFormData({
                    ...formData,
                    "operacion": "agregar",
                    "show": 1,
                })
            }}>
                Agrega
            </Button>

            <Table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Salsa</th>
                        <th>Toppings</th>
                        <th>Editar</th>
                        <th>Eliminar</th>
                    </tr>
                </thead>
                <tbody>
                    {pizzas.map(pizza =>
                        <tr key={pizza.id}>
                            <td>{pizza.name}</td>
                            <td>{pizza.sauce?.name}</td>
                            <td>{pizza.toppings?.map(topping =>
                                topping.name
                            ).join(",")
                            }</td>
                            <td>
                                <Button variant="primary" onClick={() => {
                                    setFormData({
                                        ...formData,
                                        "name": pizza.name,
                                        "id": pizza.id,
                                        "sauce": pizza.sauce?.id,
                                        "toppings": pizza.toppings?.map(t=>t.id),
                                        "operacion": "actualiza",
                                        "show": 1,
                                    })
                                }}>
                                    Editar
                                </Button>
                            </td>
                            <td>
                                <Button variant="primary" onClick={() => {
                                    setFormData({
                                        ...formData,
                                        "id": pizza.id,
                                        "name": pizza.name,
                                        "operacion": "elimina",
                                        "show": 1,
                                    })
                                }}>
                                    Elimina
                                </Button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </Table>
        </>
    );

    let contents = loading
        ? <p><em>Loading...</em></p>
        : renderPizzaTable();

    return (
        <div>
            <h1 id="tabelLabel" >Pizzas</h1>
            <p>Ejemplo de pizzas del servidor.</p>
            {contents}
            <ModalEditarPizza
                sauces={sauces}
                toppings={toppings}
                formData={formData}
                pizza={pizzas.filter(p => p.id===formData.id)[0]}
                setFormData={setFormData}
                reloadData={reloadData}
            />
        </div>
    );
}