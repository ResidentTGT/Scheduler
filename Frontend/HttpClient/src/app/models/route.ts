import { Detail } from './detail';
import { Operation } from './operation';

export class Route {
    public id?: number | {};
    public name: string;
    public description: string;
    public detail: Detail;
    public operations: Operation[];
}
