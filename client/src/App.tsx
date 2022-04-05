import Modal from 'components/modal/Modal';
import { useState } from 'react';

const App = () => {
    const [isOpen, setIsOpen] = useState(false);
    return (
        <>
            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                <h1>a</h1>
            </Modal>
            <button value="Click to pop modal" onClick={() => setIsOpen(!isOpen)}>
                Click to pop modal
            </button>
        </>
    );
};

export default App;
