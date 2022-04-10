import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';
import { GbfRaidCode } from 'types';

const MainPage = () => {
    const [codes, setCodes] = useState<string[]>([]);
    const [connection, setConnection] = useState<HubConnection>();
    const codesRef = useRef<string[]>();
    codesRef.current = codes;

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:5220/raids')
            .withAutomaticReconnect()
            .build();

        setConnection(connection);
    }, []);

    useEffect(() => {
        const run = async () => {
            if (connection) {
                await connection.start().catch((e) => console.log('Connection failed: ', e));
                console.log('success');

                connection.on(
                    'ReceiveRaidCode',
                    (perceptualHash: string, raidCode: GbfRaidCode) => {
                        console.log(raidCode.code);
                        const newCodes = codesRef.current ? [...codesRef.current] : [];
                        newCodes.push(raidCode.code);
                        setCodes(newCodes);
                    },
                );
            }
        };
        run();
    }, [connection]);

    const join = async () => {
        if (connection) {
            await connection.invoke('JoinRaid', '15382499838667692690');
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
};

export default MainPage;
