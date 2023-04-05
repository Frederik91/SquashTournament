import { useState } from 'react';
import { Button, Card, CardActions, CardContent, Dialog, DialogActions, DialogContent, DialogTitle, Grid, List, ListItem, ListItemText, TextField, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { createTournament, getTournaments } from '../services/TournamentService';

function CreateTournamentModal(props) {
    const [name, setName] = useState('');
    const [start, setStart] = useState('');
    const [end, setEnd] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        const newTournament = await createTournament({ name, start, end });
        props.onClose();
        navigate(`/tournament/${newTournament.id}`);
    };

    const handleStartChange = (e) => {
        const newStart = e.target.value;
        setStart(newStart);
        // Set the end time 5 hours after the start time only if the end field is not set
        if (!end) {
            const newEnd = new Date(newStart);
            newEnd.setHours(newEnd.getHours() + 5);
            setEnd(newEnd);
        }
    };

    return (
        <Dialog open={props.open} onClose={props.onClose}>
            <DialogTitle>Create Tournament</DialogTitle>
            <DialogContent>
                <form onSubmit={handleSubmit}>
                    <TextField
                        label="Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        fullWidth
                        margin="normal"
                        required
                    />
                    <TextField
                        label="Start"
                        type="datetime-local"
                        value={start}
                        onChange={handleStartChange}
                        fullWidth
                        margin="normal"
                        required
                        InputLabelProps={{
                            shrink: true,
                        }}
                    />
                    <TextField
                        label="End"
                        type="datetime-local"
                        value={end}
                        onChange={(e) => setEnd(e.target.value)}
                        fullWidth
                        margin="normal"
                        required
                        InputLabelProps={{
                            shrink: true,
                        }}
                    />
                    <DialogActions>
                        <Button onClick={props.onClose}>Cancel</Button>
                        <Button type="submit" variant="contained" color="primary">
                            Create
                        </Button>
                    </DialogActions>
                </form>
            </DialogContent>
        </Dialog>
    );
}

export default CreateTournamentModal;