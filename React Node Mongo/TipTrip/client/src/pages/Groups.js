import React, { useContext, useEffect, useState, useRef } from 'react';
import {
    Box, Button, TextField, Typography, Paper, List, ListItem, ListItemText, Divider, Grid, Dialog, DialogTitle, DialogContent, DialogActions
} from '@mui/material';
import { AuthContext } from '../context/AuthContext';
import io from 'socket.io-client';

const SOCKET_URL = 'http://localhost:5000';

const Groups = () => {
    const { user } = useContext(AuthContext);
    const [groups, setGroups] = useState([]);
    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [groupMessages, setGroupMessages] = useState([]);
    const [messageInput, setMessageInput] = useState('');
    const [newGroupName, setNewGroupName] = useState('');
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [editGroupName, setEditGroupName] = useState('');
    const [editGroupId, setEditGroupId] = useState('');
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [deleteGroupId, setDeleteGroupId] = useState('');
    const messagesEndRef = useRef();
    const socketRef = useRef();

    // Fetch all groups
    const fetchGroups = async () => {
        try {
            const token = localStorage.getItem('token');
            const res = await fetch('http://localhost:5000/api/groups', {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            const data = await res.json();
            setGroups(data);
        } catch (error) {
            console.error('Error fetching groups:', error);
        }
    };

    useEffect(() => {
        fetchGroups();
    }, []);

    // Socket connection and group chat logic
    useEffect(() => {
        if (!selectedGroupChat) return;

        if (!socketRef.current) {
            socketRef.current = io(SOCKET_URL);
        }
        const socket = socketRef.current;

        socket.emit('join group', selectedGroupChat._id);

        // Always fetch latest messages when opening chat
        const fetchMessages = async () => {
            try {
                const res = await fetch(`http://localhost:5000/api/groups/${selectedGroupChat._id}/messages`, {
                    headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
                });
                if (!res.ok) throw new Error('Failed to fetch messages');
                const data = await res.json();
                setGroupMessages(data);
            } catch (error) {
                console.error('Error fetching messages:', error);
            }
        };
        fetchMessages();

        socket.on('group message', (msg) => {
            setGroupMessages(prev => [...prev, msg]);
        });

        return () => {
            socket.emit('leave group', selectedGroupChat._id);
            socket.off('group message');
        };
    }, [selectedGroupChat]);

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, [groupMessages]);

    // Create new group
    const handleCreateGroup = async () => {
        if (!newGroupName.trim()) return;
        try {
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5000/api/groups', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ name: newGroupName })
            });
            if (!response.ok) throw new Error('Failed to create group');
            const newGroup = await response.json();
            setGroups(prev => [newGroup, ...prev]);
            setNewGroupName('');
        } catch (error) {
            console.error('Error creating group:', error);
        }
    };

    // Delete group (only owner) - open dialog
    const handleDeleteGroup = (groupId) => {
        setDeleteGroupId(groupId);
        setDeleteDialogOpen(true);
    };

    // Confirm delete
    const handleConfirmDeleteGroup = async () => {
        try {
            const token = localStorage.getItem('token');
            await fetch(`http://localhost:5000/api/groups/${deleteGroupId}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${token}` }
            });
            setGroups(prev => prev.filter(g => g._id !== deleteGroupId));
            if (selectedGroupChat?._id === deleteGroupId) setSelectedGroupChat(null);
            setDeleteDialogOpen(false);
            setDeleteGroupId('');
        } catch (error) {
            console.error('Error deleting group:', error);
        }
    };

    // Join group (only if not already member)
    const handleJoinGroup = async (groupId) => {
        try {
            const res = await fetch(`http://localhost:5000/api/groups/${groupId}/join`, {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
            });
            if (res.ok) fetchGroups();
        } catch (error) {
            console.error('Error joining group:', error);
        }
    };

    // Open chat for group
    const handleOpenChat = (group) => {
        setSelectedGroupChat(group);
    };

    // Edit group name (owner only)
    const handleEditGroup = (group) => {
        setEditGroupId(group._id);
        setEditGroupName(group.name);
        setEditDialogOpen(true);
    };

    const handleEditGroupSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5000/api/groups/${editGroupId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ name: editGroupName })
            });
            if (!response.ok) throw new Error('Failed to update group');
            setEditDialogOpen(false);
            setEditGroupId('');
            setEditGroupName('');
            // Update group name in UI immediately
            setGroups(prev =>
                prev.map(g => g._id === editGroupId ? { ...g, name: editGroupName } : g)
            );
            // If editing the open chat, update its name too
            if (selectedGroupChat && selectedGroupChat._id === editGroupId) {
                setSelectedGroupChat({ ...selectedGroupChat, name: editGroupName });
            }
        } catch (error) {
            console.error('Error editing group:', error);
        }
    };

    // Send message and show it immediately, and save to server
    const handleSendMessage = async (e) => {
        e.preventDefault();
        if (!messageInput.trim() || !selectedGroupChat) return;
        const msg = {
            groupId: selectedGroupChat._id,
            content: messageInput,
            userId: user._id,
            sender: { _id: user._id },
            timestamp: new Date()
        };
        setGroupMessages(prev => [...prev, msg]);
        socketRef.current?.emit('group message', msg);
        setMessageInput('');
        // Save to server for persistence
        try {
            await fetch(`http://localhost:5000/api/groups/${selectedGroupChat._id}/messages`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify({ content: msg.content })
            });
        } catch (error) {
            // Ignore error, optimistic UI
        }
    };

    // Helper to check if current user is owner
    const isOwner = (group) =>
        group.owner === user?._id || (group.owner && group.owner._id === user?._id);

    // Helper to check if current user is member
    const isMember = (group) =>
        group.members.some(m => (m._id || m) === user?._id);

    useEffect(() => {
        // If the selected group chat was deleted, clear it from state
        if (
            selectedGroupChat &&
            !groups.some(g => g._id === selectedGroupChat._id)
        ) {
            setSelectedGroupChat(null);
            setGroupMessages([]);
        }
    }, [groups, selectedGroupChat]);

    return (
        <Grid container spacing={3}>
            {/* Groups List */}
            <Grid item xs={12} md={4}>
                <Paper sx={{ p: 2 }}>
                    <Typography variant="h6" gutterBottom>
                        Groups
                    </Typography>
                    <Box sx={{ mb: 2 }}>
                        <TextField
                            fullWidth
                            size="small"
                            label="New Group Name"
                            value={newGroupName}
                            onChange={(e) => setNewGroupName(e.target.value)}
                            sx={{ mb: 1 }}
                        />
                        <Button
                            fullWidth
                            variant="contained"
                            onClick={handleCreateGroup}
                            disabled={!newGroupName.trim()}
                        >
                            Create Group
                        </Button>
                    </Box>
                    <Divider sx={{ my: 2 }} />
                    <List>
                        {groups.map((group) => (
                            <ListItem
                                key={group._id}
                                divider
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    alignItems: 'stretch',
                                    gap: 1
                                }}
                            >
                                <Box sx={{ width: '100%', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                    <ListItemText
                                        primary={group.name}
                                        secondary={`Members: ${group.members.length} ${isOwner(group) ? '(Owner)' : ''}`}
                                    />
                                </Box>
                                <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end', width: '100%' }}>
                                    {isOwner(group) && (
                                        <>
                                            <Button
                                                variant="contained"
                                                color="primary"
                                                onClick={() => handleOpenChat(group)}
                                            >
                                                Open Chat
                                            </Button>
                                            <Button
                                                variant="outlined"
                                                color="secondary"
                                                onClick={() => handleEditGroup(group)}
                                            >
                                                Edit
                                            </Button>
                                            <Button
                                                variant="outlined"
                                                color="error"
                                                onClick={() => handleDeleteGroup(group._id)}
                                            >
                                                Delete
                                            </Button>
                                        </>
                                    )}
                                    {!isOwner(group) && isMember(group) && (
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={() => handleOpenChat(group)}
                                        >
                                            Open Chat
                                        </Button>
                                    )}
                                    {!isOwner(group) && !isMember(group) && (
                                        <Button
                                            variant="outlined"
                                            color="primary"
                                            onClick={() => handleJoinGroup(group._id)}
                                        >
                                            Join Group
                                        </Button>
                                    )}
                                </Box>
                            </ListItem>
                        ))}
                    </List>
                </Paper>
            </Grid>

            {/* Chat Area */}
            {selectedGroupChat && (
                <Grid item xs={12} md={8}>
                    <Paper sx={{ p: 2, height: '80vh', display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="h6" gutterBottom>
                            {selectedGroupChat.name} Chat
                            {isOwner(selectedGroupChat) && ' (Owner)'}
                        </Typography>
                        <Box
                            sx={{
                                flex: 1,
                                overflowY: 'auto',
                                mb: 2,
                                p: 2
                            }}
                        >
                            {groupMessages.map((msg, i) => (
                                <Box
                                    key={i}
                                    sx={{
                                        display: 'flex',
                                        justifyContent: ((msg.sender?._id || msg.sender) === user._id) ? 'flex-end' : 'flex-start',
                                        mb: 1
                                    }}
                                >
                                    <Paper
                                        sx={{
                                            p: 1,
                                            backgroundColor: ((msg.sender?._id || msg.sender) === user._id) ? 'primary.light' : 'grey.100',
                                            color: ((msg.sender?._id || msg.sender) === user._id) ? 'white' : 'inherit',
                                            maxWidth: '70%'
                                        }}
                                    >
                                        <Typography variant="body2">
                                            {msg.content}
                                        </Typography>
                                        <Typography variant="caption" display="block" sx={{ opacity: 0.7 }}>
                                            {new Date(msg.timestamp).toLocaleTimeString()}
                                        </Typography>
                                    </Paper>
                                </Box>
                            ))}
                            <div ref={messagesEndRef} />
                        </Box>
                        <Box component="form" onSubmit={handleSendMessage} sx={{ mt: 2 }}>
                            <TextField
                                fullWidth
                                value={messageInput}
                                onChange={(e) => setMessageInput(e.target.value)}
                                placeholder="Type your message..."
                                variant="outlined"
                                size="small"
                            />
                            <Button
                                type="submit"
                                variant="contained"
                                sx={{ mt: 1 }}
                                fullWidth
                                disabled={!messageInput.trim()}
                            >
                                Send Message
                            </Button>
                        </Box>
                    </Paper>
                </Grid>
            )}

            {/* Edit Group Dialog */}
            <Dialog open={editDialogOpen} onClose={() => setEditDialogOpen(false)}>
                <DialogTitle>Edit Group Name</DialogTitle>
                <form onSubmit={handleEditGroupSubmit}>
                    <DialogContent>
                        <TextField
                            fullWidth
                            label="Group Name"
                            value={editGroupName}
                            onChange={e => setEditGroupName(e.target.value)}
                            required
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setEditDialogOpen(false)}>Cancel</Button>
                        <Button type="submit" variant="contained" color="primary">Save</Button>
                    </DialogActions>
                </form>
            </Dialog>

            {/* Delete Group Dialog */}
            <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
                <DialogTitle>Delete Group</DialogTitle>
                <DialogContent>
                    <Typography>
                        Are you sure you want to delete this group? This action cannot be undone.
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialogOpen(false)}>Cancel</Button>
                    <Button onClick={handleConfirmDeleteGroup} color="error" variant="contained">
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid>
    );
};

export default Groups;
