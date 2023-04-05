import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { getTournaments, createTournament } from '../services/TournamentService'
import CreateTournamentModal from '../modals/CreateTournamentModal'
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useNavigate } from 'react-router-dom';
import {
    Button,
    List,
    ListItem,
    ListItemAvatar,
    Avatar,
    ListItemText,
    Card,
    CardActionArea,
    CardContent,
    Grid,
    Typography,
    CardActions,
} from "@mui/material";

const theme = createTheme();


function HomePage() {
    const [data, setData] = useState([]);
    const [isCreateTournamentModalOpen, setIsCreateTournamentModalOpen] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchDataAsync = async () => {
            const response = await getTournaments();
            setData(response);
        };

        fetchDataAsync();
    }, []);

    // Navigate to the tournament page
    const handleOpenTournament = (id) => {
        navigate(`/tournament/${id}`);
    }


    // Open the Create Tournament modal
    const handleOpenCreateTournamentModal = () => {
        setIsCreateTournamentModalOpen(true);
    };

    // Close the Create Tournament modal
    const handleCloseCreateTournamentModal = () => {
        setIsCreateTournamentModalOpen(false);
    };

    return (

        <ThemeProvider theme={theme}>
            <Grid container spacing={2}>
                <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                    <Button onClick={handleOpenCreateTournamentModal}>Create Tournament</Button>
                </Grid>
                <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
                    {data.map((item) => (
                        <ListItem alignItems="flex-start" key={item.Id}>
                            <Card >
                                <CardContent>
                                    <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                        Tournament
                                    </Typography>
                                    <Typography variant="h5" component="div">
                                        {item.name}
                                    </Typography>
                                    <Typography
                                        sx={{ display: 'inline' }}
                                        component="span"
                                        variant="body2"
                                        color="text.primary"
                                    >
                                        Start: {new Date(item.start).toLocaleDateString(navigator.language, {
                                            day: '2-digit',
                                            month: '2-digit',
                                            year: 'numeric',
                                            hour: 'numeric',
                                            minute: 'numeric'
                                        })}
                                        <br />
                                        End: {new Date(item.end).toLocaleDateString(navigator.language, {
                                            day: '2-digit',
                                            month: '2-digit',
                                            year: 'numeric',
                                            hour: 'numeric',
                                            minute: 'numeric'
                                        })}
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                    <Button size="small" onClick={() => handleOpenTournament(item.id)} >Open</Button>
                                </CardActions>
                            </Card>
                        </ListItem>
                    ))}
                </List>
                <CreateTournamentModal
                    open={isCreateTournamentModalOpen}
                    onClose={handleCloseCreateTournamentModal}
                />
            </Grid>
        </ThemeProvider>
    );
}

export default HomePage;
