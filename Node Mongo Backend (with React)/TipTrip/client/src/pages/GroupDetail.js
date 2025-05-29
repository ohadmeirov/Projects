import React, { useEffect, useState } from 'react';
import io from 'socket.io-client';
const socket = io('http://localhost:5000');

const GroupDetail = ({ groupId }) => {
    const [group, setGroup] = useState(null);
    const [message, setMessage] = useState('');
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        fetch(`/api/groups/${groupId}`, { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` } })
            .then(res => res.json())
            .then(setGroup);
        socket.emit('join group', groupId);
        socket.on('group message', msg => setMessages(prev => [...prev, msg]));
        return () => socket.emit('leave group', groupId);
    }, [groupId]);

    const sendMessage = (e) => {
        e.preventDefault();
        socket.emit('group message', { groupId, text: message });
        setMessage('');
    };

    return group ? (
        <div style={{ display: 'flex' }}>
            <div style={{ flex: 2 }}>
                <h2>{group.name} - Group Chat</h2>
                <div style={{ height: 200, overflowY: 'auto', border: '1px solid #ccc' }}>
                    {messages.map((msg, i) => <div key={i}>{msg.user}: {msg.text}</div>)}
                </div>
                <form onSubmit={sendMessage}>
                    <input value={message} onChange={e => setMessage(e.target.value)} required />
                    <button type="submit">Send</button>
                </form>
            </div>
            <div style={{ flex: 1, marginLeft: 16 }}>
                <h3>Members</h3>
                <ul>
                    {group.members.map(member => (
                        <li key={member._id}>
                            {member.username}
                            <Button size="small" onClick={() => followUser(member._id)}>Follow</Button>
                            <Button size="small" onClick={() => openPrivateChat(member._id)}>Chat</Button>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    ) : <div>Loading...</div>;
};

export default GroupDetail;
