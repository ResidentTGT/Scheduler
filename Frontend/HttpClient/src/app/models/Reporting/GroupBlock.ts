import { DetailsBatchBlock } from './DetailsBatchBlock';

export class GroupBlock {
    public groupIndex: number;

    public workshopId: number;

    public startTime: number;

    public duration: number;

    public detailsBatchBlocks: DetailsBatchBlock[];
}
