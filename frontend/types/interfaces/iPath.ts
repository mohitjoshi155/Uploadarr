import IPathType from './iPathType';

export default interface IPath {
	path: string;
	type: IPathType;
	name?: string;
	freespace?: number;
	unmappedfolders?: number;
	size?: number;
}
