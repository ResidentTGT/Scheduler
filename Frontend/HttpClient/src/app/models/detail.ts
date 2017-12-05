import { Route } from './route';

export class Detail {
    public id?: number | {};
    public title: string;
    public description: string;
    public cost?: number;
    public isPurchased?: Boolean = false;
    public routeName?: string;
    public workshopSequence?: number[];
    public equipmentsIdSequence?: number[];
    public equipmentsNameSequence?: string[];
}
