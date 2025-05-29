import React, { useContext } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import {
    AppBar,
    Toolbar,
    Typography,
    Button,
    Box,
    IconButton,
    Avatar,
    Menu,
    MenuItem,
    useMediaQuery,
    useTheme
} from '@mui/material';
import {
    Home as HomeIcon,
    Add as AddIcon,
    AccountCircle,
    Logout
} from '@mui/icons-material';
import { AuthContext } from '../context/AuthContext';

const Navbar = () => {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const [anchorEl, setAnchorEl] = React.useState(null);

    const handleMenu = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        handleClose();
        logout();
        navigate('/login');
    };

    return (
        <AppBar position="sticky">
            <Toolbar>
                <Typography
                    variant="h6"
                    component={RouterLink}
                    to="/"
                    sx={{
                        flexGrow: 1,
                        textDecoration: 'none',
                        color: 'inherit',
                        display: 'flex',
                        alignItems: 'center'
                    }}
                >
                    <HomeIcon sx={{ mr: 1 }} />
                    TipTrip
                </Typography>

                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                    {user && (
                        <Typography variant="body1" sx={{ mr: 2 }}>
                            Hello, {user.username}!
                        </Typography>
                    )}
                    <Button
                        color="inherit"
                        component={RouterLink}
                        to="/create-post"
                    >
                        Create Post
                    </Button>
                    <Button
                        color="inherit"
                        component={RouterLink}
                        to="/groups"
                    >
                        Groups
                    </Button>
                    <Button
                        color="inherit"
                        component={RouterLink}
                        to="/stats"
                    >
                        Statistics
                    </Button>

                    {user ? (
                        <>
                            <IconButton
                                size="large"
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                onClick={handleMenu}
                                color="inherit"
                            >
                                <Avatar
                                    alt={user.username}
                                    src={user.avatar}
                                    sx={{ width: 32, height: 32 }}
                                />
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorEl}
                                anchorOrigin={{
                                    vertical: 'bottom',
                                    horizontal: 'right',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                open={Boolean(anchorEl)}
                                onClose={handleClose}
                            >
                                <MenuItem
                                    component={RouterLink}
                                    to={`/profile/${user._id}`}
                                    onClick={handleClose}
                                >
                                    <AccountCircle sx={{ mr: 1 }} />
                                    Profile
                                </MenuItem>
                                <MenuItem onClick={handleLogout}>
                                    <Logout sx={{ mr: 1 }} />
                                    Logout
                                </MenuItem>
                            </Menu>
                        </>
                    ) : (
                        <>
                            <Button
                                color="inherit"
                                component={RouterLink}
                                to="/login"
                            >
                                Login
                            </Button>
                            <Button
                                color="inherit"
                                component={RouterLink}
                                to="/register"
                            >
                                Register
                            </Button>
                        </>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;
