import { Context } from '@nuxt/types';

/*
 ** vuetify module configuration
 ** https://github.com/nuxt-community/vuetify-module
 */
export default (ctx: Context): void => {
	ctx.$axios.setBaseURL('http://localhost:5000/api/');
};
