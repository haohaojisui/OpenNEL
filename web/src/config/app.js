const getProtocol = () => {
  if (typeof window !== 'undefined' && window.location) {
    return window.location.protocol === 'https:' ? 'wss:' : 'ws:'
  }
  return 'ws:'
}
const getPort = () => {
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_PORT') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_NEL_PORT : null
  return fromStorage || fromEnv || null
}
const getHost = () => (typeof window !== 'undefined' && window.location && window.location.hostname) ? window.location.hostname : '127.0.0.1'
const getDefault = () => `${getHost()}:8080`
export const getWsUrl = () => {
  const proto = getProtocol()
  const port = getPort()
  const host = getHost()
  const authority = port ? `${host}:${port}` : getDefault()
  return `${proto}//${authority}/ws`
}
const getRandomNameUrl = () => {
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_RANDOM_NAME_URL') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_RANDOM_NAME_URL : null
  return fromStorage || fromEnv || '/nel-random-name'
}
const getAnnouncementUrl = () => {
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_ANNOUNCEMENT_URL') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_ANNOUNCEMENT_URL : null
  const isDev = typeof import.meta !== 'undefined' && import.meta.env && import.meta.env.DEV
  return fromStorage || fromEnv || (isDev ? '/nel-announcement' : 'http://23.140.4.9:3498/announcement/get')
}
const getAnnouncementTTL = () => {
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_ANNOUNCEMENT_TTL') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_ANNOUNCEMENT_TTL : null
  const v = fromStorage || fromEnv
  const n = v ? parseInt(v, 10) : NaN
  return Number.isFinite(n) && n > 0 ? n : 300000
}
const getNoticeKey = () => {
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_NOTICE_KEY') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_NOTICE_KEY : null
  return fromStorage || fromEnv || 'NEL_NOTICE_ACK'
}
const getNoticeText = () => {
  const def = 'OpenNEL  Copyright (C) 2025 OpenNEL Studio\n本程序是自由软件，你可以重新发布或修改它，但必须：\n- 保留原始版权声明\n- 采用相同许可证分发\n- 提供完整的源代码'
  const fromStorage = typeof localStorage !== 'undefined' ? localStorage.getItem('NEL_NOTICE_TEXT') : null
  const fromEnv = typeof import.meta !== 'undefined' ? import.meta.env && import.meta.env.VITE_NOTICE_TEXT : null
  return fromStorage || fromEnv || def
}
export default { getWsUrl, getRandomNameUrl, getAnnouncementUrl, getAnnouncementTTL, getNoticeKey, getNoticeText }