import axios from 'axios';
import { Button, Form, Modal } from 'react-bootstrap';

export default function ModalEditarPizza(props) {
    const handleInputChange = e => {
        props.setFormData({ ...props.formData, [e.target.name]: e.target.value })
    }

    const hideForm = () => {
        props.setFormData({
            ...props.formData,
            "operacion": "",
            "id": "",
            "name": "",
            "sauce": "",
            "toppings": [],
            "show": 0
        });
    }

    const updatePizza = async (data) => {
        if(data.toppings == null){
            data.toppings = [];
        }
        // console.log(data);
        // console.log(props.sauces)
        const pizza = {
            "id": data.id == null || data.id=="" ? 0 : data.id,
            "name": data.name,
            "sauce": parseInt(data.sauce),
            "toppings": data.toppings.map(x => parseInt(x)),
        }
        console.log(pizza);
        if (data.operacion === "elimina") {
            await axios.delete(`api/pizza/${data.id}`);
        }
        else if (data.operacion === "actualiza") {
            await axios.post(`api/pizza/updatePizza`, pizza).then(r  => console.log(r));
        }
        else if (data.operacion === "agregar") {
            await axios.post(`api/pizza`, pizza).then(r => console.log(r));
        }
        return;
    }

    const saveAndHideForm = async () => {
        async function update() {
            await updatePizza(props.formData);
            hideForm();
            props.reloadData();
        }
        update();
    }

    let content = props.formData.operacion === "elimina" ?
        <>
            <p> Eliminar pizza: {props.formData.name}</p>
        </> :
        <Form>
            <Form.Group >
                <Form.Label>ID: </Form.Label>
                <Form.Control
                    type="text"
                    name="id"
                    onChange={handleInputChange}
                    value={props.formData.id}
                    placeholder="ID" />
            </Form.Group>
            <Form.Group >
                <Form.Label>Name: </Form.Label>
                <Form.Control
                    type="text"
                    name="name"
                    onChange={handleInputChange}
                    value={props.formData.name}
                    placeholder="Name" />
            </Form.Group>
            <Form.Group >
                <Form.Label>Sauce: </Form.Label>
                <Form.Select
                    name="sauce"
                    onChange={handleInputChange}
                    value={props.formData.sauce}>
                    <option>Salsas</option>
                    {props.sauces.map(sauce =>
                        <option value={sauce.id}>{sauce.name}</option>
                    )}
                </Form.Select>
            </Form.Group>
            <Form.Group >
                <Form.Label>Toppings: </Form.Label>
                <Form.Select
                    name="toppings"
                    onChange={
                        e =>
                            props.setFormData({
                                ...props.formData, [e.target.name]:
                                    [].slice.call(e.target.selectedOptions).map(item => parseInt(item.value))
                            })
                    }
                    multiple
                    value={props.formData.toppings}
                >
                    {props.toppings.map(topping =>
                        <option value={topping.id}>{topping.name}</option>
                    )}
                </Form.Select>
            </Form.Group>
        </Form>

    return (
        <Modal show={props.formData.show !== 0} onHide={hideForm}>
            <Modal.Header closeButton>
                <Modal.Title>Editar pizza</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {content}
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={hideForm}>
                    Close
                </Button>
                <Button variant="primary" onClick={saveAndHideForm}>
                    Save Changes
                </Button>
            </Modal.Footer>
        </Modal>
    );
}