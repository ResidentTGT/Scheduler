import { DetailsBatchBlock } from './DetailsBatchBlock';
import { Workshop } from '../workshop';

export class GroupBlock {
    public id: number;

    public groupIndex: number;

    public workshop: Workshop;

    public startTime: number;

    public duration: number;

    public transportStartTime: number;

    public transportDuration: number;

    public detailsBatchBlocks: DetailsBatchBlock[];
}
