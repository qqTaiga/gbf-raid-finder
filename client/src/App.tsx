import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';

function App() {
    const [codes, setCodes] = useState<string[]>([]);
    const [connection, setConnection] = useState<HubConnection>();
    const codesRef = useRef<string[]>();
    codesRef.current = codes;

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:5220/raids')
            .withAutomaticReconnect()
            .build();

        connection.on('messageReceived', (code: string) => {
            const newCodes = codesRef.current ? [...codesRef.current] : [];
            newCodes.push(code);
            setCodes(newCodes);
        });

        setConnection(connection);
    }, []);

    useEffect(() => {
        const run = async () => {
            if (connection) {
                await connection.start().catch((e) => console.log('Connection failed: ', e));
                console.log('success');
            }
        };
        run();
    }, [connection]);

    const join = async () => {
        if (connection) {
            await connection.invoke('JoinRaidRoomAsync', 'test');
        }
    };

    return (
        <div className="App">
            <div className="App-header">
                <table>
                    <tbody>
                        <tr>
                            <td>Test</td>
                        </tr>
                    </tbody>
                </table>
                <table>
                    <tbody>
                        {codes.map((code, index) => (
                            <tr key={index}>
                                <td>
                                    <p>{code}</p>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            <button onClick={() => join()}>Join</button>
        </div>
    );
}

export default App;
