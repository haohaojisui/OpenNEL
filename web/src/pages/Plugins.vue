<template>
  <div class="plugins-page">
    <div class="card">
      <div class="card-title">
        已安装插件
        <div class="actions">
          <button class="btn" @click="restartGateway">重启</button>
        </div>
      </div>
      <div class="card-body">
        <div v-if="installed.length === 0" class="empty">暂无已安装插件</div>
        <div class="list">
          <div v-for="p in installed" :key="p.identifier || p.name" class="row">
            <div class="info">
              <div class="name">{{ p.name }}</div>
              <div class="version">{{ p.version }}</div>
              <div class="status" v-if="p.waitingRestart">已卸载，等待重启</div>
            </div>
            <div class="actions">
              <button class="btn danger" @click="uninstallPlugin(p.identifier)">卸载</button>
            </div>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import appConfig from '../config/app.js'
const installed = ref([])
let socket
function requestInstalled() {
  if (!socket || socket.readyState !== 1) return
  try { socket.send(JSON.stringify({ type: 'list_installed_plugins' })) } catch {}
}
function uninstallPlugin(id) {
  if (!id) return
  if (!socket || socket.readyState !== 1) return
  try { socket.send(JSON.stringify({ type: 'uninstall_plugin', pluginId: id })) } catch {}
}
function restartGateway() {
  if (!socket || socket.readyState !== 1) return
  try { socket.send(JSON.stringify({ type: 'restart' })) } catch {}
}
onMounted(() => {
  try {
    socket = new WebSocket(appConfig.getWsUrl())
    socket.onopen = () => { requestInstalled() }
    socket.onmessage = (e) => {
      let msg
      try { msg = JSON.parse(e.data) } catch { msg = null }
      if (!msg || !msg.type) return
      if (msg.type === 'installed_plugins' && Array.isArray(msg.items)) {
        installed.value = (msg.items || []).map(it => ({
          identifier: it.identifier,
          name: it.name,
          version: it.version,
          waitingRestart: !!it.waitingRestart
        }))
      } else if (msg.type === 'installed_plugins_updated') {
        requestInstalled()
      }
    }
  } catch {}
})
onUnmounted(() => { try { if (socket && socket.readyState === 1) socket.close() } catch {} })
</script>

<style scoped>
.plugins-page { display: flex; flex-direction: column; gap: 16px; width: 100%; align-self: flex-start; margin-right: auto; }
.card { border: 1px solid var(--glass-border); border-radius: 12px; background: var(--glass-surface); color: var(--color-text); backdrop-filter: blur(var(--glass-blur)); }
.card-title { display: flex; align-items: center; justify-content: space-between; padding: 12px 16px; font-size: 16px; font-weight: 600; border-bottom: 1px solid var(--glass-border); }
.card-body { padding: 12px 16px; }
.empty { opacity: 0.7; }
.list { display: grid; grid-template-columns: 1fr; gap: 8px; }
.row { display: flex; align-items: center; justify-content: space-between; border: 1px solid var(--glass-border); border-radius: 12px; padding: 12px; background: var(--glass-surface); backdrop-filter: blur(var(--glass-blur)); }
.actions { display: flex; gap: 8px; }
.btn { padding: 8px 12px; border: 1px solid var(--glass-border); background: var(--glass-surface); color: var(--color-text); border-radius: 8px; cursor: pointer; transition: opacity 200ms ease, transform 100ms ease; backdrop-filter: blur(var(--glass-blur)); }
.btn:hover { opacity: 0.9; }
.btn:active { transform: scale(0.98); }
.btn.danger { color: #ef4444; }
.status { font-size: 12px; color: #f59e0b; }
.info { display: flex; flex-direction: column; gap: 4px; }
.name { font-size: 14px; font-weight: 600; }
.version { font-size: 12px; opacity: 0.7; }
</style>