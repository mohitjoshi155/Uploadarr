import Vue from 'vue';
import Vuetify from 'vuetify';
import { Context } from '@nuxt/types';
import 'vuetify/dist/vuetify.min.css';
import 'material-design-icons-iconfont/dist/material-design-icons.css';
import '@mdi/font/css/materialdesignicons.css'; // Ensure you are using css-loader version "^2.1.1" ,

/*
 ** vuetify module configuration
 ** https://github.com/nuxt-community/vuetify-module
 */
export default (ctx: Context): void => {
	Vue.use(Vuetify);

	const vuetify = new Vuetify({
		theme: {
			dark: true, // From 2.0 You have to select the theme dark or light here
		},
		icons: {
			iconfont: 'mdi',
		},
	});

	ctx.app.vuetify = vuetify;
	ctx.$vuetify = vuetify.framework;
};
