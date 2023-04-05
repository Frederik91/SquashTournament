import { get, post } from './RestClientWrapper'

export const getTournaments = async () => {
    const tournaments = await get("tournaments");
    return tournaments;
}

export const createTournament = async (tournament = {}) => {
    const createdTournament = await post("tournaments", tournament);
    return createdTournament;
}